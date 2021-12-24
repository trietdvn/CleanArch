using AutoMapper;
using CleanArch.Domain.Dtos;
using CleanArch.Domain.Entities;

namespace CleanArch.Api.Mappings
{
    public class MapppingProfile : Profile
    {
        public MapppingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}