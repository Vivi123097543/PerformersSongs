using MediatR;
using PerformersSongs.Application.DTOs;
using PerformersSongs.Application.Interfaces;

namespace PerformersSongs.Application.Features.Performers.Queries;

public class GetPerformerByIdQueryHandler
    : IRequestHandler<GetPerformerByIdQuery, PerformerDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPerformerByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PerformerDto?> Handle(
        GetPerformerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var performer = await _unitOfWork.Performers
            .GetByIdAsync(request.Id);

        if (performer == null)
            return null;

        return new PerformerDto
        {
            Id = performer.Id,
            Name = performer.Name,
            Country = performer.Country,
            Genre = performer.Genre,
            FoundationDate = performer.FoundationDate,
            PhotoPath = performer.PhotoPath
        };
    }
}
