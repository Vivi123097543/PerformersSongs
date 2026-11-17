using MediatR;
using PerformersSongs.Application.DTOs;
using PerformersSongs.Application.Interfaces;

namespace PerformersSongs.Application.Features.Songs.Queries;

public class GetSongByIdQueryHandler
    : IRequestHandler<GetSongByIdQuery, SongDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSongByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SongDto?> Handle(
        GetSongByIdQuery request,
        CancellationToken cancellationToken)
    {
        var song = await _unitOfWork.Songs.GetByIdAsync(request.Id);

        if (song == null)
            return null;

        return new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ChartPosition = song.ChartPosition,
            ReleaseYear = song.ReleaseYear,
            Duration = song.Duration,
            PerformerId = song.PerformerId
        };
    }
}
