using AutoMapper;

namespace App.BLL;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        
        CreateMap<App.DAL.DTO.City, App.BLL.DTO.City>().ReverseMap();
        
        CreateMap<App.DAL.DTO.ExtraFee, App.BLL.DTO.ExtraFee>().ReverseMap();
        
        CreateMap<App.DAL.DTO.StandardFee, App.BLL.DTO.StandardFee>().ReverseMap();
        
        CreateMap<App.DAL.DTO.VehicleType, App.BLL.DTO.VehicleType>().ReverseMap();
        
        CreateMap<App.DAL.DTO.Weather, App.BLL.DTO.Weather>().ReverseMap();
        
        CreateMap<App.DAL.DTO.Station, App.BLL.DTO.Station>().ReverseMap();
        
    }
    
}