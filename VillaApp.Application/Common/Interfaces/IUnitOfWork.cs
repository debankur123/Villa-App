namespace VillaApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IVillaRepository villaRepo { get; }
    IVillaNumberRepository villaNumberRepo { get; }
    IAmenity amenityRepo { get; }
    void CommitToDb();
}