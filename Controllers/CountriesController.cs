using AutoMapper;
using HotelListing.DTO;
using HotelListing.DTO.Country;
using HotelListing.Exceptions;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Controllers;

[Route("api/[controller]")]
// [Route("api/v{version:apiVersion}/[controller]")]
// [Route("api/v{version:apiVersion/ countries")]
[ApiController]
// [Authorize]
[ApiVersion("1.0")]  // api/Countries?api-version=1.0 OR send the version in the header
public class CountriesController : ControllerBase
{
    private readonly ICountriesRepository _countriesRepository;
    private readonly IMapper _mapper;

    public CountriesController(ICountriesRepository countriesRepository, IMapper mapper)
    {
        _countriesRepository = countriesRepository;
        _mapper = mapper;
    }

    // GET: api/Countries
    [HttpGet("getAll")]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync();
        var result = _mapper.Map<List<GetCountryDto>>(countries);
        return result;
    }

    // GET: api/Countries?StartIndex=0&PageSize=25&PageNumber=1
    [HttpGet]    
    public async Task<ActionResult<PagedResult<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
    {
        return await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
    }
    
    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetails(id);

        if (country == null) throw new NotFoundException(nameof(GetCountry), id);

        var result = _mapper.Map<CountryDto>(country);

        return result;
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountrydto)
    {
        if (id != updateCountrydto.Id) return BadRequest();

        var country = await _countriesRepository.GetAsync(id);
        if (country is null) return NotFound();

        _mapper.Map(updateCountrydto, country);

        try
        {
            await _countriesRepository.UpdateAsync(country);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _countriesRepository.Exists(id)) throw new NotFoundException(nameof(PutCountry), id); 
            else throw;
        }

        return NoContent();
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
    {
        var country = _mapper.Map<Country>(createCountryDto);

        await _countriesRepository.AddAsync(country);

        return CreatedAtAction("GetCountry", new { id = country.Id }, country);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);

        if (country == null) throw new NotFoundException(nameof(DeleteCountry), id);

        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }
}
