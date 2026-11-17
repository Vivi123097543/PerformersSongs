using MediatR;

namespace PerformersSongs.Application.Features.Songs.Commands;

public class DeleteSongCommand : IRequest
{
    public int Id { get; set; }

    public DeleteSongCommand(int id)
    {
        Id = id;
    }
}
