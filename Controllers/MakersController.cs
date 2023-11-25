using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Data.AsyncDataServices;
using ProfileService.Data.Interfaces;
using ProfileService.Data.SyncDataServices.Http;
using ProfileService.Domain.DTOs;
using ProfileService.Domain.Models;

namespace ProfileService.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MakersController : ControllerBase
{
    private readonly IMakerRepo _repo;
    private readonly IMapper _mapper;
    private readonly IProductDataClient _productDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public MakersController(
        IMakerRepo repo,
        IMapper mapper,
        IProductDataClient productDataClient,
        IMessageBusClient messageBusClient)
    {
        _repo = repo;
        _mapper = mapper;
        _productDataClient = productDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MakerReadDto>> GetMakers()
    {
        Console.WriteLine("---> Getting Makers");
        
        var makers = _repo.GetAllMakers();
        
        return Ok(_mapper.Map<IEnumerable<Maker>>(makers));
    }

    [HttpGet("{id}", Name = "GetMakerById")]
    public ActionResult<Maker> GetMakerById(int id)
    {
        var maker = _repo.GetMakerById(id);
        if (maker != null)
            return Ok(_mapper.Map<Maker>(maker));

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<MakerReadDto>> CreateMaker(MakerCreateDto makerDto)
    {
        var makerModel = _mapper.Map<Maker>(makerDto);
        _repo.CreateMaker(makerModel);
        _repo.SaveChanges();

        var makerReadDto = _mapper.Map<MakerReadDto>(makerModel);

        // Send Sync Message
        try
        {
            await _productDataClient.SendMakerToProduct(makerReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"---> Could not send synchronously: {ex.Message}");
        }
        
        // Send Async Message
        try
        {
            var makerPublishedDto = _mapper.Map<MakerPublishedDto>(makerReadDto);
            makerPublishedDto.Event = "Maker_Published";
            _messageBusClient.PublishNewMaker(makerPublishedDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"---> Could not send asynchronously: {ex.Message}");
        }
        return CreatedAtRoute(nameof(GetMakerById), new { Id = makerReadDto.Id }, makerReadDto);
    }
}