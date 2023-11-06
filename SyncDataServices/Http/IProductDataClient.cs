using ProfileService.Domain.DTOs;

namespace ProfileService.SyncDataServices.Http;

public interface IProductDataClient
{
    Task SendMakerToProduct(MakerReadDto maker);
}