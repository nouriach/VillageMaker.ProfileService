using Microsoft.EntityFrameworkCore;
using ProfileService.Data.Interfaces;
using ProfileService.Domain.Models;

namespace ProfileService.Data.Repositories;

public class ProfileRepo : IProfileRepo
{
    private readonly AppDbContext _context;

    public ProfileRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }

    public IEnumerable<Profile> GetAllProfiles()
    {
        return _context.Profiles.ToList();
    }

    public Profile GetProfileById(int id)
    {
        return _context.Profiles.FirstOrDefault(x => x.Id == id);
    }

    public void CreatePlatform(Profile profile)
    {
        if (profile == null)
            throw new ArgumentNullException();

        _context.Profiles.Add(profile);
    }
}