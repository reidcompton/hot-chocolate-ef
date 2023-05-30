using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class DataAccessServiceFactory<T> where T : class
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        public DataAccessServiceFactory(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }


        public virtual T Create()
        {
            return (T)Activator.CreateInstance(typeof(T), _dbFactory)!;
        }
    }


    public class DataAccessService<DbModel> : IAsyncDisposable where DbModel : class
    {
        protected readonly ApplicationDbContext _db;
        public DataAccessService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }


        public int Delete(int id)
        {
            try
            {
                DbModel? item = _db.Find<DbModel>(id);
                if (item != null)
                    _db.Remove(item);
                return id;
            }
            catch (Exception ex) { throw OnException(ex); }
        }


        public async Task<DbModel?> GetAsync(int id)
        {
            try
            {
                return await _db.FindAsync<DbModel>(id);
            }
            catch (Exception ex) { throw OnException(ex); }
        }


        public IEnumerable<DbModel> List(int offset = 0, int limit = int.MaxValue)
        {
            try
            {
                return _db.Set<DbModel>().Skip(offset).Take(limit);
            }
            catch (Exception ex) { throw OnException(ex); }
        }


        public async Task<DbModel> CreateAsync(DbModel model)
        {
            try
            {
                await _db.AddAsync(model);
                return model;
            }
            catch (Exception ex) { throw OnException(ex); }
        }


        public async Task<DbModel?> UpdateAsync(DbModel model)
        {
            try
            {
                // get id, find by id
                BaseEntity? be = model as BaseEntity;
                DbModel? existing = await _db.FindAsync<DbModel>(be?.Id);
                // preserve created date
                if (existing != null)
                {
                    var createdDate = typeof(DbModel).GetProperty("CreatedDate")?.GetValue(existing);
                    typeof(DbModel).GetProperty("CreatedDate")?.SetValue(model, createdDate);
                    // update entry with new values
                    _db.Entry(existing).CurrentValues.SetValues(model);
                }
                return _db.Find<DbModel>(be?.Id);
            }
            catch (Exception ex) { throw OnException(ex); }
        }


        private Exception OnException(Exception ex)
        {
            RollBack();
            throw ex;
        }
        private void RollBack()
        {
            var changedEntries = _db.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();


            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }


        public async ValueTask DisposeAsync()
        {
            if (_db.ChangeTracker.HasChanges())
                await _db.SaveChangesAsync();
        }
    }
}

