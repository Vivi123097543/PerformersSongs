using MediatR;
using PerformersSongs.Application.DTOs;

namespace PerformersSongs.Application.Features.Songs.Queries;

public class GetSongByIdQuery : IRequest<SongDto?>
{
    public int Id { get; set; }

    public GetSongByIdQuery(int id)
    {
        Id = id;
    }
}
