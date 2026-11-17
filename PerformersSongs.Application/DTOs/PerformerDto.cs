namespace PerformersSongs.Application.DTOs;

public class PerformerDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public DateTime FoundationDate { get; set; }

    public string? PhotoPath { get; set; }
}
