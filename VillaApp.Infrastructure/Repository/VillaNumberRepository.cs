using VillaApp.Application.Common.Interfaces;
using VillaApp.Domains.Entities;
using VillaApp.Infrastructure.Data;

namespace VillaApp.Infrastructure.Repository;

public class VillaNumberRepository : GenericRepository<VillaNumber>,IVillaNumberRepository
{
    private readonly ApplicationDbContext _context;
    public VillaNumberRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(VillaNumber entity)
    {
        _context!.Tbl_VillaNumber.Update(entity);
    }
}