using MediatR;
using PerformersSongs.Application.Interfaces;
using PerformersSongs.Domain.Entities;

namespace PerformersSongs.Application.Features.Performers.Commands;

public class AddPerformerCommandHandler
    : IRequestHandler<AddPerformerCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddPerformerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        AddPerformerCommand request,
        CancellationToken cancellationToken)
    {
        var performer = new Performer
        {
            Name = request.Name,
            Country = request.Country,
            Genre = request.Genre,
            FoundationDate = request.FoundationDate,
            PhotoPath = request.PhotoPath
        };

        await _unitOfWork.Performers.AddAsync(performer);
        await _unitOfWork.SaveChangesAsync();

        return performer.Id;
    }
}
