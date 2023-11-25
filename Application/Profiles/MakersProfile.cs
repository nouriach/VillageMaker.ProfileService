using AutoMapper;
using ProfileService.Domain.DTOs;
using ProfileService.Domain.Models;

namespace ProfileService.Application.Profiles;

public class MakersProfile : Profile
{
    public MakersProfile()
    {
        // Source -> Target
        // Source = what comes from the Request
        // Target = what is returned from the DB
        CreateMap<Maker, MakerReadDto>();
        CreateMap<MakerCreateDto, Maker>();
        CreateMap<MakerReadDto, MakerPublishedDto>();
    }
}