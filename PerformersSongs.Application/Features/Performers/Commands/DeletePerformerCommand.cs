using MediatR;

namespace PerformersSongs.Application.Features.Performers.Commands;

public class DeletePerformerCommand : IRequest
{
    public int Id { get; set; }

    public DeletePerformerCommand(int id)
    {
        Id = id;
    }
}
