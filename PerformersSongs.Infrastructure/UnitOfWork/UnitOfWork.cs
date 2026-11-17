using PerformersSongs.Application.Interfaces;
using PerformersSongs.Infrastructure.Data;
using PerformersSongs.Infrastructure.Repositories;

namespace PerformersSongs.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IPerformerRepository Performers { get; }

    public ISongRepository Songs { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        Performers = new PerformerRepository(context);
        Songs = new SongRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
