using System.Text;
using System.Text.Json;
using ProfileService.Domain.DTOs;

namespace ProfileService.Data.SyncDataServices.Http;

public class HttpProductDataClient : IProductDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public HttpProductDataClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }
    public async Task SendMakerToProduct(MakerReadDto maker)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(maker),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync($"{_config["ProductService"]}", httpContent);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("---> Sync POST to Product Service was OK");
        }
        else
        {
            Console.WriteLine("---> Sync POST to Product Service was unsuccessful");
        }
    }
}