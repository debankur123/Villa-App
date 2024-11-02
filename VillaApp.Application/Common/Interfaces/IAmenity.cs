using VillaApp.Domains.Entities;

namespace VillaApp.Application.Common.Interfaces;

public interface IAmenity : IGenericRepository<Amenity>
{
    void Update(Amenity entity);
}