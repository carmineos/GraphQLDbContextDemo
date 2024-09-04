using HotChocolate.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApplication1.GraphQL;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Again, just an example using EF but you do not have to
builder.Services.AddDbContext<DemoContext>(opt => opt.UseInMemoryDatabase("Demo"));

builder.Services
    .AddGraphQLServer()
    .BindRuntimeType<uint, UnsignedIntType>()
    .RegisterDbContext<DemoContext>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddType(new UuidType('D'))
    .AddQueryType<Queries>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapPost("/movies", async (Movie movie, DemoContext context, CancellationToken cancellationToken) =>
{
    context.Movies.Add(movie);

    await context.SaveChangesAsync(cancellationToken);
})
.WithName("AddMovie")
.WithOpenApi();

app.MapPost("/query", async ([FromBody] QueryRequest request, IRequestExecutorResolver requestResolver, CancellationToken cancellationToken) =>
{
    request.PathJsonElementStringVariables();

    var executor = await requestResolver.GetRequestExecutorAsync();

    var result = await executor.ExecuteAsync(request.Query, request.Variables, cancellationToken);

    return result.ToJson();
})
.WithName("Query")
.WithOpenApi();

app.MapGraphQL();

app.Run();

public class QueryRequest
{
    public string Query { get; set; } = default!;
    public Dictionary<string, object?> Variables { get; set; } = [];

    public void PathJsonElementStringVariables()
    {
        foreach (var key in Variables.Keys)
        {
            if (Variables[key] is not JsonElement { ValueKind: JsonValueKind.String } elem)
                continue;

            if (elem.TryGetGuid(out var guid)) Variables[key] = guid as Guid?;
            if (elem.TryGetDateTime(out var datetime)) Variables[key] = datetime as DateTime?;
            if (elem.TryGetDateTimeOffset(out var datetimeoffset)) Variables[key] = datetimeoffset as DateTimeOffset?;
        }
    }
}

/* Esempio
{
    "query": "query ($movieId: UUID!) {  movie(id: $movieId) {   name  }}",
  "variables": {
        "movieId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
*/