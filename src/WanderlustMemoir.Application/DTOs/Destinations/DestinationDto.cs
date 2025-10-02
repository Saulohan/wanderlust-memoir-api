namespace WanderlustMemoir.Application.DTOs.Destinations;

public class DestinationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsVisited { get; set; }
    public string Priority { get; set; } = string.Empty;
    public List<DestinationPhotoDto> Photos { get; set; } = new();
    public DateTime? DateVisited { get; set; }
    public DateTime? VisitDate { get; set; }
    public string? Description { get; set; }
    public int? Rating { get; set; }
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
    public DateTime? DateVisited { get; set; }
    public string? Description { get; set; }
    public int? Rating { get; set; }
}

public class DestinationPhotoDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DateTaken { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ToggleVisitedDto
{
    public string? VisitDate { get; set; }
}

public class UpdateRatingDto
{
    public int Rating { get; set; }
}

public class UpdatePriorityDto
{
    public string Priority { get; set; } = string.Empty;
}