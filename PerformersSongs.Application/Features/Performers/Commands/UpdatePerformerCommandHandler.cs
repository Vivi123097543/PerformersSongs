using MediatR;
using PerformersSongs.Application.Interfaces;

namespace PerformersSongs.Application.Features.Performers.Commands;

public class UpdatePerformerCommandHandler
    : IRequestHandler<UpdatePerformerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePerformerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdatePerformerCommand request,
        CancellationToken cancellationToken)
    {
        var performer = await _unitOfWork.Performers
            .GetByIdAsync(request.Id);

        if (performer == null)
            throw new KeyNotFoundException(
                $"Исполнитель с ID {request.Id} не найден.");

        performer.Name = request.Name;
        performer.Country = request.Country;
        performer.Genre = request.Genre;
        performer.FoundationDate = request.FoundationDate;
        performer.PhotoPath = request.PhotoPath;

        _unitOfWork.Performers.Update(performer);

        await _unitOfWork.SaveChangesAsync();
    }
}
