using VillaApp.Application.Common.Interfaces;
using VillaApp.Domains.Entities;
using VillaApp.Infrastructure.Data;

namespace VillaApp.Infrastructure.Repository;

public class AmenityRepository : GenericRepository<Amenity>,IAmenity
{
    private readonly ApplicationDbContext _context;

    public AmenityRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Amenity entity)
    {
        _context.Tbl_Amenities.Update(entity);
    }
}