using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Models;

namespace ProfileService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
        
    }

    public DbSet<Maker> Makers { get; set; }
}