using Person.Data;
using Person.Models;

namespace Person.Routes;
public static class PersonRoute
{
    public static void PersonRoutes(this WebApplication app)
    {
        var routes = app.MapGroup("person");

        routes.MapPost("", 
            async (PersonRequest req, PersonContext context) =>
            {
                var person = new PersonModel(req.name);
                await context.AddAsync(person);
                await context.SaveChangesAsync();
            } 
        );
    }
}