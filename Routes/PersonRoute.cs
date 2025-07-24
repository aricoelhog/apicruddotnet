using Microsoft.EntityFrameworkCore;
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

        routes.MapGet("", 
            async (PersonContext context) => {
            var people = await context.People.ToListAsync();
            return Results.Ok(people);
        });

        routes.MapPut("{id:guid}", 
            async(Guid id, PersonRequest req, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
                // var person = await context.People.FindAsync(id);
                if (person == null)
                    return Results.NotFound();

                person.ChangeName(req.name);
                await context.SaveChangesAsync();
                return Results.Ok(person);
            }
        );

        routes.MapDelete("{id:guid}",
            async (Guid id, PersonContext context) =>
            {
                var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                    return Results.NotFound();

                person.SetInactive();
                await context.SaveChangesAsync();
                return Results.Ok(person);
            }
        );
    }
}