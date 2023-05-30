using Api.Data;
using Api.Services;
using System;

namespace Api.Graph.Users
{
    public class UserDataLoaders
    {
        public class UserBatchDataLoader : BatchDataLoader<int, User>
        {
            private readonly DataAccessServiceFactory<UserService> _userServiceFactory;

            public UserBatchDataLoader(
                DataAccessServiceFactory<UserService> userServiceFactory,
                IBatchScheduler batchScheduler,
                DataLoaderOptions? options = null)
                : base(batchScheduler, options)
            {
                _userServiceFactory = userServiceFactory;
            }

            protected override async Task<IReadOnlyDictionary<int, User>> LoadBatchAsync(
                IReadOnlyList<int> keys,
                CancellationToken cancellationToken)
            {
                await using var service = _userServiceFactory.Create();
                var users = service.GetUserByIds(keys);
                return users.ToDictionary(x => x.Id);
            }
        }
    }
}
