using MediatR;
using PerformersSongs.Application.Interfaces;

namespace PerformersSongs.Application.Features.Songs.Commands;

public class UpdateSongCommandHandler
    : IRequestHandler<UpdateSongCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSongCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateSongCommand request,
        CancellationToken cancellationToken)
    {
        var song = await _unitOfWork.Songs.GetByIdAsync(request.Id);

        if (song == null)
            throw new KeyNotFoundException(
                $"Песня с ID {request.Id} не найдена.");

        song.Title = request.Title;
        song.ChartPosition = request.ChartPosition;
        song.ReleaseYear = request.ReleaseYear;
        song.Duration = request.Duration;
        song.PerformerId = request.PerformerId;

        _unitOfWork.Songs.Update(song);

        await _unitOfWork.SaveChangesAsync();
    }
}
