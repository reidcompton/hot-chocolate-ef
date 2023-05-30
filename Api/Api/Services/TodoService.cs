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
    }
}
