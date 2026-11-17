using MediatR;

namespace PerformersSongs.Application.Features.Songs.Commands;

public class AddSongCommand : IRequest<int>
{
    public string Title { get; set; } = string.Empty;

    public int ChartPosition { get; set; }

    public int ReleaseYear { get; set; }

    public TimeSpan Duration { get; set; }

    public int PerformerId { get; set; }
}
