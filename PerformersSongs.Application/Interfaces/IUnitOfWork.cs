namespace PerformersSongs.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPerformerRepository Performers { get; }

    ISongRepository Songs { get; }

    Task<int> SaveChangesAsync();
}
