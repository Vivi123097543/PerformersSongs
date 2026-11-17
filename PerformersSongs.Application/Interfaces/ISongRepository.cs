using PerformersSongs.Domain.Entities;

namespace PerformersSongs.Application.Interfaces;

public interface ISongRepository
{
    Task<List<Song>> GetAllAsync();

    Task<List<Song>> GetByPerformerIdAsync(int performerId);

    Task<Song?> GetByIdAsync(int id);

    Task AddAsync(Song song);

    void Update(Song song);

    void Delete(Song song);
}
