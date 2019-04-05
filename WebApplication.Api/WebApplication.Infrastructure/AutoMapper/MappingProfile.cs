using AutoMapper;
using WebApplication.Core.Domain;
using WebApplication.Infrastructure.DTO;

namespace WebApplication.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AccountDTO>();
            CreateMap<Voivodeship, VoivodeshipDTO>();
            CreateMap<City, CityDTO>();
        }
    }
}
