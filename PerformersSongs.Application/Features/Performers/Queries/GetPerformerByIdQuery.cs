using MediatR;
using PerformersSongs.Application.DTOs;

namespace PerformersSongs.Application.Features.Performers.Queries;

public class GetPerformerByIdQuery : IRequest<PerformerDto?>
{
    public int Id { get; set; }

    public GetPerformerByIdQuery(int id)
    {
        Id = id;
    }
}
