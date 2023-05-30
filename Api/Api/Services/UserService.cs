using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserService : DataAccessService<User>
    {
        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory): base(dbFactory) { }

        public IEnumerable<User> GetUserByIds(IReadOnlyList<int> keys)
        {
            return _db.Users.Where(x => keys.Contains(x.Id));
        }
    }
}
