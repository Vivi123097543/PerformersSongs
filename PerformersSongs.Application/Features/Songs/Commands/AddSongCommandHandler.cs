using MediatR;
using PerformersSongs.Application.Interfaces;
using PerformersSongs.Domain.Entities;

namespace PerformersSongs.Application.Features.Songs.Commands;

public class AddSongCommandHandler
    : IRequestHandler<AddSongCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddSongCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        AddSongCommand request,
        CancellationToken cancellationToken)
    {
        var song = new Song
        {
            Title = request.Title,
            ChartPosition = request.ChartPosition,
            ReleaseYear = request.ReleaseYear,
            Duration = request.Duration,
            PerformerId = request.PerformerId
        };

        await _unitOfWork.Songs.AddAsync(song);
        await _unitOfWork.SaveChangesAsync();

        return song.Id;
    }
}
