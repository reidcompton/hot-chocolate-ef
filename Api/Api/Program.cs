using Api.Data;
using Api.Graph;
using Api.Graph.Todos;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using static Api.Graph.Todos.TodoDataLoaders;
using static Api.Graph.Users.UserDataLoaders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options => 
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=hotchocolate-ef-dev; Trusted_connection=true")
);
builder.Services.AddGraphQLServer()
                .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
                .AddQueryType<Query>()
                .AddTypeExtension<TodoQueries>()
                .AddType<TodoType>()
                .AddDataLoader<TodoByUserIdDataLoader>()
                .AddDataLoader<UserBatchDataLoader>()
                .AddFiltering();


builder.Services.AddTransient<DataAccessServiceFactory<TodoService>>();
builder.Services.AddTransient<DataAccessServiceFactory<UserService>>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbService = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    var db = dbService.CreateDbContext();
    if (db.Database.GetPendingMigrations().Any())
    {
        db.Database.Migrate();
    }
    if (!db.Users.Any())
    {
        db.Users.AddRange(new List<User>
        {
            new User {
                FirstName = "foo",
                LastName = "bar",
                Todos = new List<Todo>
                {
                    new Todo { IsDone = true, Description = "this is a todo"},
                    new Todo { IsDone = true, Description = "this is a todo"},
                    new Todo { IsDone = true, Description = "this is a todo"},
                    new Todo { IsDone = false, Description = "this is a todo"},
                    new Todo { IsDone = false, Description = "this is a todo"},
                    new Todo { IsDone = false, Description = "this is a todo"}
                }
            },
            new User {
                FirstName = "baz",
                LastName = "bat",
                Todos = new List<Todo>
                {
                    new Todo { IsDone = true, Description = "this is a todo"},
                    new Todo { IsDone = true, Description = "this is a todo"},
                    new Todo { IsDone = true, Description = "this is a todo"},
                    new Todo { IsDone = false, Description = "this is a todo"},
                    new Todo { IsDone = false, Description = "this is a todo"},
                    new Todo { IsDone = false, Description = "this is a todo"}
                }
            } 
        });
        db.SaveChanges();
    }
}

app.MapGraphQL();

app.Run();
