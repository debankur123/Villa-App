
using System.Linq.Expressions;
using VillaApp.Domains.Entities;

namespace VillaApp.Application.Common.Interfaces;
public interface IVillaRepository
{
    IEnumerable<Villa> GetVillas(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null);
    Villa GetVilla(Expression<Func<Villa, bool>> filter, string? includeProperties = null);
    void Add(Villa entity);
    void Update(Villa entity);
    void SaveToDB();
}
