using Api.Data;
using Api.Graph;
using Api.Graph.Todos;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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
                .AddType<TodoType>();

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
    var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    db.CreateDbContext().Database.Migrate();
}

app.MapGraphQL();

app.Run();
