using Microsoft.EntityFrameworkCore;
using PerformersSongs.Application.Interfaces;
using PerformersSongs.Domain.Entities;
using PerformersSongs.Infrastructure.Data;

namespace PerformersSongs.Infrastructure.Repositories;

public class PerformerRepository : IPerformerRepository
{
    private readonly AppDbContext _context;

    public PerformerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Performer>> GetAllAsync()
    {
        return await _context.Performers
            .Include(p => p.Songs)
            .ToListAsync();
    }

    public async Task<Performer?> GetByIdAsync(int id)
    {
        return await _context.Performers
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Performer performer)
    {
        await _context.Performers.AddAsync(performer);
    }

    public void Update(Performer performer)
    {
        _context.Performers.Update(performer);
    }

    public void Delete(Performer performer)
    {
        _context.Performers.Remove(performer);
    }
}
