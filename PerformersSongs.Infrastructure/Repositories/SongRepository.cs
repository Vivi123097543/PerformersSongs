using Microsoft.EntityFrameworkCore;
using PerformersSongs.Application.Interfaces;
using PerformersSongs.Domain.Entities;
using PerformersSongs.Infrastructure.Data;

namespace PerformersSongs.Infrastructure.Repositories;

public class SongRepository : ISongRepository
{
    private readonly AppDbContext _context;

    public SongRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Song>> GetAllAsync()
    {
        return await _context.Songs
            .Include(s => s.Performer)
            .ToListAsync();
    }

    public async Task<List<Song>> GetByPerformerIdAsync(int performerId)
    {
        return await _context.Songs
            .Where(s => s.PerformerId == performerId)
            .ToListAsync();
    }

    public async Task<Song?> GetByIdAsync(int id)
    {
        return await _context.Songs
            .Include(s => s.Performer)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Song song)
    {
        await _context.Songs.AddAsync(song);
    }

    public void Update(Song song)
    {
        _context.Songs.Update(song);
    }

    public void Delete(Song song)
    {
        _context.Songs.Remove(song);
    }
}
