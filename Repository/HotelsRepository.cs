using AutoMapper;
using HotelListing.IRepository;

namespace HotelListing.Repository;

public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
{
    public HotelsRepository(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {    }
}
