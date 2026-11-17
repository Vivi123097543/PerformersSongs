using MediatR;
using PerformersSongs.Application.DTOs;
using PerformersSongs.Application.Interfaces;

namespace PerformersSongs.Application.Features.Performers.Queries;

public class GetAllPerformersQueryHandler
    : IRequestHandler<GetAllPerformersQuery, List<PerformerDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPerformersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PerformerDto>> Handle(
     GetAllPerformersQuery request,
     CancellationToken cancellationToken)
    {
        var performers = await _unitOfWork.Performers.GetAllAsync();

        System.Diagnostics.Debug.WriteLine(
            $"Количество исполнителей в БД: {performers.Count}");

        return performers.Select(p => new PerformerDto
        {
            Id = p.Id,
            Name = p.Name,
            Country = p.Country,
            Genre = p.Genre,
            FoundationDate = p.FoundationDate,
            PhotoPath = p.PhotoPath
        }).ToList();
    }
}