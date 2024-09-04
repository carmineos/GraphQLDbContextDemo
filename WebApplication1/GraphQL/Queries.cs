using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.GraphQL;

public class Queries
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Movie> GetMovies([Service(ServiceKind.Synchronized)] DemoContext context)
         => context.Movies.AsNoTracking();

    [UseProjection]
    public IQueryable<Movie> GetMovie([Service(ServiceKind.Synchronized)] DemoContext context, Guid id)
     => context.Movies.AsNoTracking().Where(m => m.Id == id);

    //public IQueryable<Person> GetPeople([Service(ServiceKind.Synchronized)] DemoContext context)
    //     => context.People.AsNoTracking();
}