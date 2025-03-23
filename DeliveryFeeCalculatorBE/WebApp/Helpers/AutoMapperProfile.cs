using AutoMapper;

namespace WebApp.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.BLL.DTO.City, App.DTO.v1_0.City>().ReverseMap();
        CreateMap<App.BLL.DTO.ExtraFee, App.DTO.v1_0.ExtraFee>().ReverseMap();
        CreateMap<App.BLL.DTO.StandardFee, App.DTO.v1_0.StandardFee>().ReverseMap();
        CreateMap<App.BLL.DTO.VehicleType, App.DTO.v1_0.VehicleType>().ReverseMap();
        CreateMap<App.BLL.DTO.Weather, App.DTO.v1_0.Weather>().ReverseMap();
        CreateMap<App.BLL.DTO.Station, App.DTO.v1_0.Station>().ReverseMap();
        
    }
}