using Api.Data;
using Api.Services;
using System;

namespace Api.Graph.Todos
{
    public class TodoDataLoaders
    {
        public class TodoByUserIdDataLoader : GroupedDataLoader<int, Todo>
        {
            private readonly DataAccessServiceFactory<TodoService> _todoServiceFactory;

            public TodoByUserIdDataLoader(
                DataAccessServiceFactory<TodoService> todoServiceFactory,
                IBatchScheduler batchScheduler,
                DataLoaderOptions? options = null)
                : base(batchScheduler, options)
            {
                _todoServiceFactory = todoServiceFactory;
            }

            protected override async Task<ILookup<int, Todo>> LoadGroupedBatchAsync(
                IReadOnlyList<int> userIds,
                CancellationToken cancellationToken)
            {
                await using var service = _todoServiceFactory.Create();
                var todos = service.GetTodosByUserId(userIds);
                return todos.ToLookup(x => x.UserId);
            }
        }
    }
}
