using Microsoft.EntityFrameworkCore;
using ProfileService.Data.Interfaces;
using ProfileService.Domain.Models;

namespace ProfileService.Data.Repositories;

public class MakerRepo : IMakerRepo
{
    private readonly AppDbContext _context;

    public MakerRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }

    public IEnumerable<Maker> GetAllMakers()
    {
        return _context.Makers.ToList();
    }

    public Maker GetMakerById(int id)
    {
        return _context.Makers.FirstOrDefault(x => x.Id == id);
    }

    public void CreateMaker(Maker maker)
    {
        if (maker == null)
            throw new ArgumentNullException(nameof(maker));

        _context.Makers.Add(maker);
    }
}