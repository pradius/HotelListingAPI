using AutoMapper;
using HotelListing.DTO.Country;
using HotelListing.DTO.Hotels;
using HotelListing.DTO.Users;

namespace HotelListing.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // Maps from Country to CreateCountryDto and viceversa 
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<Country, UpdateCountryDto>().ReverseMap();

        CreateMap<Hotel, HotelsDto>().ReverseMap();
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();

        CreateMap<ApiUser, ApiUserDto>().ReverseMap();
    }
        
}
