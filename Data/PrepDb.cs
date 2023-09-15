namespace ProfileService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }
    }

    private static void SeedData(AppDbContext context)
    {
        if (!context.Profiles.Any())
        {
            Console.WriteLine("---> Seeding data...");
            context.Profiles.AddRange(
                );

        }
        else
        {
            Console.WriteLine("---> We already have data");
        }
            
    }
}