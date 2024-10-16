using VillaApp.Application.Common.Interfaces;
using VillaApp.Domains.Entities;
using VillaApp.Infrastructure.Data;
namespace VillaApp.Infrastructure.Repository;

public class VillaRepository : GenericRepository<Villa>,IVillaRepository
{
    private readonly ApplicationDbContext? _context; 
    public VillaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Villa entity)
    {
        _context!.Tbl_Villa.Update(entity);
    }
    // public void SaveToDb()
    // {
    //     _context!.SaveChanges();
    // }
}
