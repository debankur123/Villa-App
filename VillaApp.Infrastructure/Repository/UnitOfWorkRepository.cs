using VillaApp.Application.Common.Interfaces;
using VillaApp.Infrastructure.Data;

namespace VillaApp.Infrastructure.Repository;

public class UnitOfWorkRepository : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IVillaRepository villaRepo { get; private set; }
    public void CommitToDb()
    {
        _context.SaveChanges();
    }

    public UnitOfWorkRepository(ApplicationDbContext context)
    {
        _context = context;
        villaRepo = new VillaRepository(_context);
    }
}