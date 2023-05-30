using Api.Data;
using Api.Services;

namespace Api.Graph.Todos
{
    [ExtendObjectType(typeof(Query))]
    public class TodoQueries
    {
        [UsePaging(MaxPageSize = int.MaxValue)]
        public async Task<IQueryable<Todo>> GetTodosAsync(
            [ID(nameof(User))] int userId,
            [Service] DataAccessServiceFactory<TodoService> todoService)
        {
            await using var service = todoService.Create();
            var todos = service.GetByUserId(userId);
            return todos;
        }

        public async Task<Todo?> GetTodoAsync(
            int id,
            [Service] DataAccessServiceFactory<TodoService> todoService)
        {
            await using var service = todoService.Create();
            var todo = await service.GetAsync(id);
            return todo;
            //return await context.Todos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
