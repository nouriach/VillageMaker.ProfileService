using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Models;

namespace ProfileService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
        }
    }

    private static void SeedData(AppDbContext context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("---> Attempting to apply Migrations");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        if (!context.Makers.Any())
        {
            Console.WriteLine("---> Seeding data...");
            context.Makers.AddRange(
                new Maker
                {
                    FirstName = "Test",
                    LastName = "One",
                    Postcode = "NN60NU"
                },
                new Maker
                {
                    FirstName = "Test",
                    LastName = "Two",
                    Postcode = "NN60JD"  
                },
                new Maker
                {
                    FirstName = "Test",
                    LastName = "Three",
                    Postcode = "NN60JJ"    
                }
            );
            
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("---> We already have data");
        }
    }
}