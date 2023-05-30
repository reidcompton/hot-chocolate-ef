using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class TodoService : DataAccessService<Todo>
    {
        public TodoService(IDbContextFactory<ApplicationDbContext> dbFactory): base(dbFactory) { }

        public IQueryable<Todo> GetByUserId(int userId)
        {
            return _db.Todos.Where(x => x.UserId == userId);
        }

        public IEnumerable<Todo> GetTodoByIds(IReadOnlyList<int> keys)
        {
            return _db.Todos.Where(x => keys.Contains(x.Id));
        }

        public IEnumerable<Todo> GetTodosByUserId(IReadOnlyList<int> userIds)
        {
            return _db.Todos.Where(x => userIds.Contains(x.UserId));
        }
    }
}
