using VillaApp.Domains.Entities;

namespace VillaApp.Application.Common.Interfaces;
public interface IVillaRepository : IGenericRepository<Villa>
{
    void Update(Villa entity);
    void SaveToDb();
}
