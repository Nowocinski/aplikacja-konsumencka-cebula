using AutoMapper;
using System.Linq;
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
            CreateMap<Advertisement, AdvertisementDetailsDTO>()
                .ForMember(
                    x => x.FirstName,
                    y => y.MapFrom(src => src.User.FirstName)
                )
            .ForMember(
                    x => x.LastName,
                    y => y.MapFrom(src => src.User.LastName)
                )
            .ForMember(
                    x => x.PhoneNumber,
                    y => y.MapFrom(src => src.User.PhoneNumber)
                )
            .ForMember(
                    x => x.Email,
                    y => y.MapFrom(src => src.User.Email)
                )
            .ForMember(
                    x => x.City,
                    y => y.MapFrom(src => src.City.Name)
                )
            .ForMember(
                    x => x.Voivodeship,
                    y => y.MapFrom(src => src.City.Voivodeship.Name)
                );
            CreateMap<User, AdvertisementDetailsDTO>();
            CreateMap<AdvertisementImage, ImageDTO>();
            CreateMap<Advertisement, AdvertismentDTO>()
               .ForMember(
                    x => x.Image,
                    y => y.MapFrom(src => src.Images.FirstOrDefault())
                )
                .ForMember(
                    x => x.City,
                    y => y.MapFrom(src => src.City.Name)
                )
                .ForMember(
                    x => x.Voivodeship,
                    y => y.MapFrom(src => src.City.Voivodeship.Name)
                );
            CreateMap<Message, MessagesDTO>()
                .ForMember(
                    x => x.FirstName,
                    y => y.MapFrom(src => src.Sender.FirstName)
                )
                .ForMember(
                    x => x.LastName,
                    y => y.MapFrom(src => src.Sender.LastName)
                )
                .ForMember(
                    x => x.Content,
                    y => y.MapFrom(src => src.Contents)
                );
        }
    }
}
