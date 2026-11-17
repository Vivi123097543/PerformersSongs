using MediatR;
using PerformersSongs.Application.DTOs;

namespace PerformersSongs.Application.Features.Songs.Queries;

public class GetSongsByPerformerIdQuery : IRequest<List<SongDto>>
{
    public int PerformerId { get; set; }

    public GetSongsByPerformerIdQuery(int performerId)
    {
        PerformerId = performerId;
    }
}
