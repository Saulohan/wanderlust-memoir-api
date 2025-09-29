namespace WanderlustMemoir.Application.DTOs.Destinations;

public class DestinationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsVisited { get; set; }
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateDestinationDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Priority { get; set; } = "medium";
}

public class UpdateDestinationDto
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public bool? IsVisited { get; set; }
    public string? Priority { get; set; }
}