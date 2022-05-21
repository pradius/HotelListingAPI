using HotelListing.DTO.Hotels;

namespace HotelListing.DTO.Country;

public class CountryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }

    public List<HotelsDto> Hotels { get; set; }
}
