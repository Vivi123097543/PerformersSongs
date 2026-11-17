using MediatR;
using PerformersSongs.Application.Interfaces;

namespace PerformersSongs.Application.Features.Songs.Commands;

public class DeleteSongCommandHandler
    : IRequestHandler<DeleteSongCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSongCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteSongCommand request,
        CancellationToken cancellationToken)
    {
        var song = await _unitOfWork.Songs.GetByIdAsync(request.Id);

        if (song == null)
            throw new KeyNotFoundException(
                $"Песня с ID {request.Id} не найдена.");

        _unitOfWork.Songs.Delete(song);

        await _unitOfWork.SaveChangesAsync();
    }
}
