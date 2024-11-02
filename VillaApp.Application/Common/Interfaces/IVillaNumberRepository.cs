using VillaApp.Domains.Entities;

namespace VillaApp.Application.Common.Interfaces;

public interface IVillaNumberRepository : IGenericRepository<VillaNumber>
{
    void Update(VillaNumber entity);
}