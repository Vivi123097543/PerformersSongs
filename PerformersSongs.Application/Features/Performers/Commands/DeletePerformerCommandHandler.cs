using MediatR;
using PerformersSongs.Application.Interfaces;

namespace PerformersSongs.Application.Features.Performers.Commands;

public class DeletePerformerCommandHandler
    : IRequestHandler<DeletePerformerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePerformerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeletePerformerCommand request,
        CancellationToken cancellationToken)
    {
        var performer = await _unitOfWork.Performers
            .GetByIdAsync(request.Id);

        if (performer == null)
            throw new KeyNotFoundException(
                $"Исполнитель с ID {request.Id} не найден.");

        _unitOfWork.Performers.Delete(performer);

        await _unitOfWork.SaveChangesAsync();
    }
}
