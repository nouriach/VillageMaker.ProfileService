
using ProfileService.Domain.Models;

namespace ProfileService.Data.Interfaces;

public interface IProfileRepo
{
    bool SaveChanges();
    IEnumerable<Profile> GetAllProfiles();
    Profile GetProfileById(int id);
    void CreatePlatform(Profile profile);
}