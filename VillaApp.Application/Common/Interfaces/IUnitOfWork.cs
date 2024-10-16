namespace VillaApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IVillaRepository villaRepo { get; }
    void CommitToDb();
}