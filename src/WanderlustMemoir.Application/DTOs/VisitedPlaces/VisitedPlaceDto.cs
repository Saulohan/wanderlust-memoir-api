namespace WanderlustMemoir.Application.DTOs.VisitedPlaces;

public class VisitedPlaceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string Date { get; set; } = string.Empty; // Para compatibilidade com o frontend
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; }
    public List<VisitedPlacePhotoDto> Photos { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class CreateVisitedPlaceDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string? Date { get; set; } // Para compatibilidade com o frontend
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public List<VisitedPlacePhotoDto>? Photos { get; set; }
}

public class UpdateVisitedPlaceDto
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public DateTime? VisitDate { get; set; }
    public string? Date { get; set; } // Para compatibilidade com o frontend
    public string? Description { get; set; }
    public int? Rating { get; set; }
    public List<VisitedPlacePhotoDto>? Photos { get; set; }
}

public class VisitedPlacePhotoDto
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

public class UpdateVisitedPlaceRatingDto
{
    public int Rating { get; set; }
}