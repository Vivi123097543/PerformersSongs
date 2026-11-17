using PerformersSongs.Domain.Entities;

namespace PerformersSongs.Application.Interfaces;

public interface IPerformerRepository
{
    Task<List<Performer>> GetAllAsync();

    Task<Performer?> GetByIdAsync(int id);

    Task AddAsync(Performer performer);

    void Update(Performer performer);

    void Delete(Performer performer);
}
