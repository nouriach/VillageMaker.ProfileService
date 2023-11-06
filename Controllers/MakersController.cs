using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Data.Interfaces;
using ProfileService.Domain.DTOs;
using ProfileService.Domain.Models;
using ProfileService.SyncDataServices.Http;

namespace ProfileService.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MakersController : ControllerBase
{
    private readonly IMakerRepo _repo;
    private readonly IMapper _mapper;
    private readonly IProductDataClient _productDataClient;

    public MakersController(IMakerRepo repo, IMapper mapper, IProductDataClient productDataClient)
    {
        _repo = repo;
        _mapper = mapper;
        _productDataClient = productDataClient;
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

        try
        {
            await _productDataClient.SendMakerToProduct(makerReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"---> Could not send synchronously: {ex.Message}");
        }
        
        return CreatedAtRoute(nameof(GetMakerById), new { Id = makerReadDto.Id }, makerReadDto);
    }
}