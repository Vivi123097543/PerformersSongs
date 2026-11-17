using MediatR;
using PerformersSongs.Application.DTOs;

namespace PerformersSongs.Application.Features.Performers.Queries;

public class GetAllPerformersQuery : IRequest<List<PerformerDto>>
{
}
