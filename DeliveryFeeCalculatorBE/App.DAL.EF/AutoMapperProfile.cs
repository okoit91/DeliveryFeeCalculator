using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    
    {
        CreateMap<App.Domain.City, App.DAL.DTO.City>().ReverseMap();
        CreateMap<App.Domain.ExtraFee, App.DAL.DTO.ExtraFee>().ReverseMap();
        CreateMap<App.Domain.StandardFee, App.DAL.DTO.StandardFee>().ReverseMap();
        CreateMap<App.Domain.VehicleType, App.DAL.DTO.VehicleType>().ReverseMap();
        CreateMap<App.Domain.Weather, App.DAL.DTO.Weather>().ReverseMap();
        CreateMap<App.Domain.Station, App.DAL.DTO.Station>().ReverseMap();
        CreateMap<App.Domain.Identity.AppUser, App.DAL.DTO.AppUser>().ReverseMap();
        CreateMap<App.Domain.Identity.AppRole, App.DAL.DTO.AppRole>().ReverseMap();
        
    }
    
}