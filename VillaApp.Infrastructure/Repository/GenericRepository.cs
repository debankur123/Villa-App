using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Infrastructure.Data;

namespace VillaApp.Infrastructure.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    internal readonly DbSet<T> entitySet;
    private static readonly char[] Separator = [','];

    protected GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        entitySet = _context.Set<T>();
    }
    public void Add(T entity)
    {
        entitySet.Add(entity);
    }

    public Boolean Any(Expression<Func<T, bool>> filter)
    {
        return entitySet.Any(filter);
    }

    public T GetVilla(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = entitySet;
        if (filter is not null)
        {
            query = query.Where(filter);
        }
        if (!string.IsNullOrEmpty(includeProperties))
        {
            String[] properties = includeProperties.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < properties.Length; i++)
            {
                query = query.Include(properties[i]);
            }
        }
        return query.FirstOrDefault()!;
    }

    public IEnumerable<T> GetVillas(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = entitySet;
        if (filter is not null)
        {
            query = query.Where(filter);
        }
        if (!string.IsNullOrEmpty(includeProperties))
        {
            String[] properties = includeProperties.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < properties.Length; i++)
            {
                query = query.Include(properties[i]);
            }
        }
        return [.. query];
    }
}
