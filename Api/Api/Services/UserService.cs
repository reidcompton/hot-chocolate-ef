using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserService : DataAccessService<User>
    {
        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory): base(dbFactory) { }
    }
}
