using AutoMapper;
using Business.Dtos;
using Domain.Entities.Data;
using Domain.Entities.Lookup;

namespace Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WatchItemDto, WatchItem>()
                .ReverseMap();
            CreateMap<UserDto, User>()
                .ReverseMap();
            CreateMap<Movie, MovieDto>()
                .ReverseMap();
        }
    }
}
