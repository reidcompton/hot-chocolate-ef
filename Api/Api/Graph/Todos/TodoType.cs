using Api.Data;
using Api.Services;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace Api.Graph.Todos
{
    public class TodoType : ObjectType<Todo>
    {
        protected override void Configure(IObjectTypeDescriptor<Todo> descriptor)
        {
            descriptor.ImplementsNode().IdField(x => x.Id);

            descriptor
                .Field(x => x.User)
                .ResolveWith<Resolvers>(x =>
                    Resolvers.GetUser(default!, default!)
                );
        }

        private class Resolvers
        {
            public static async Task<User?> GetUser(
                [Parent] Todo todo,
                [Service] DataAccessServiceFactory<UserService> userService)
            {
                await using var service = userService.Create();
                return await service.GetAsync(todo.UserId);
            }
        }
    }
}
