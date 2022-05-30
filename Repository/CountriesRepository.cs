using AutoMapper;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly DatabaseContext _context;

    public CountriesRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    public async Task<Country> GetDetails(int id)
    {
        return await _context.Countries.Include(x => x.Hotels).FirstOrDefaultAsync(x => x.Id == id);
    }
}
