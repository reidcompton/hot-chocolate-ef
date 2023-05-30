using Api.Data;
using Api.Services;
using HotChocolate.Types;
using System;

namespace Api.Graph.Users
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
        }
    }
}
