using ProfileService.Domain.DTOs;

namespace ProfileService.Data.SyncDataServices.Http;

public interface IProductDataClient
{
    Task SendMakerToProduct(MakerReadDto maker);
}