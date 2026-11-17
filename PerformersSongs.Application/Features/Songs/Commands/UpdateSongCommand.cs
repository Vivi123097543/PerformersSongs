using MediatR;

namespace PerformersSongs.Application.Features.Songs.Commands;

public class UpdateSongCommand : IRequest
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int ChartPosition { get; set; }

    public int ReleaseYear { get; set; }

    public TimeSpan Duration { get; set; }

    public int PerformerId { get; set; }
}
