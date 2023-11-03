
using ProfileService.Domain.Models;

namespace ProfileService.Data.Interfaces;

public interface IMakerRepo
{
    bool SaveChanges();
    IEnumerable<Maker> GetAllMakers();
    Maker GetMakerById(int id);
    void CreateMaker(Maker maker);
}