using MediatR;
using PerformersSongs.Application.DTOs;
using PerformersSongs.Application.Interfaces;

namespace PerformersSongs.Application.Features.Songs.Queries;

public class GetSongsByPerformerIdQueryHandler
    : IRequestHandler<GetSongsByPerformerIdQuery, List<SongDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSongsByPerformerIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SongDto>> Handle(
        GetSongsByPerformerIdQuery request,
        CancellationToken cancellationToken)
    {
        var songs = await _unitOfWork.Songs
            .GetByPerformerIdAsync(request.PerformerId);

        return songs.Select(s => new SongDto
        {
            Id = s.Id,
            Title = s.Title,
            ChartPosition = s.ChartPosition,
            ReleaseYear = s.ReleaseYear,
            Duration = s.Duration,
            PerformerId = s.PerformerId
        }).ToList();
    }
}
