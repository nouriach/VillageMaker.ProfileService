using ProfileService.Domain.DTOs;

namespace ProfileService.Data.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewMaker(MakerPublishedDto makerPublishedDto);
}