
using System.Linq.Expressions;

namespace VillaApp.Application.Common.Interfaces;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T> GetVillas(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    T GetVilla(Expression<Func<T, bool>> filter, string? includeProperties = null);
    void Add(T entity);
    Boolean Any(Expression<Func<T, bool>> filter);
}
