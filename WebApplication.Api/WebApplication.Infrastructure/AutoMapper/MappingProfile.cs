using AutoMapper;
using System.Linq;
using WebApplication.Core.Domain;
using WebApplication.Infrastructure.Commands;
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
            CreateMap<Advertisement, AdvertisementDetailsDTO>();
            CreateMap<User, AdvertisementDetailsDTO>();
            CreateMap<AdvertisementImage, ImageDTO>();
            CreateMap<Advertisement, AdvertismentDTO>()
               .ForMember(
                    x => x.Image,
                    y => y.MapFrom(src => src.Images.FirstOrDefault())
                );
        }
    }
}
