using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Domains.Entities;
using VillaApp.Infrastructure.Data;
namespace VillaApp.Infrastructure.Repository;

public class VillaRepository(ApplicationDbContext context) : IVillaRepository
{
    private static readonly char[] separator = [','];

    public void Add(Villa entity)
    {
        context.Tbl_Villa.Add(entity);
    }

    public Villa GetVilla(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
    {
        IQueryable<Villa> query = context.Set<Villa>();
        if (filter is not null)
        {
            query = query.Where(filter);
        }
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var item in includeProperties.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }
        return query.FirstOrDefault()!;
    }

    public IEnumerable<Villa> GetVillas(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<Villa> query = context.Set<Villa>();
        if (filter is not null)
        {
            query = query.Where(filter);
        }
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var item in includeProperties.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }
        return [.. query];
    }

    public void SaveToDB()
    {
        context.SaveChanges();
    }

    public void Update(Villa entity)
    {
        context.Tbl_Villa.Update(entity);
    }
}
