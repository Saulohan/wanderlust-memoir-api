namespace WanderlustMemoir.Application.DTOs.VisitedPlaces;

public class VisitedPlaceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; }
    public List<PhotoDto> Photos { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class CreateVisitedPlaceDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
}

public class UpdateVisitedPlaceDto
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public DateTime? VisitDate { get; set; }
    public string? Description { get; set; }
    public int? Rating { get; set; }
}

public class PhotoDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public DateTime CreatedAt { get; set; }
}