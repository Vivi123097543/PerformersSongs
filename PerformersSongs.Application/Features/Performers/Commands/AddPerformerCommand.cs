using MediatR;

namespace PerformersSongs.Application.Features.Performers.Commands;

public class AddPerformerCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public DateTime FoundationDate { get; set; }

    public string? PhotoPath { get; set; }
}
