using WanderlustMemoir.Domain.Common;

namespace WanderlustMemoir.Domain.Entities;

public class Photo : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int VisitedPlaceId { get; set; }
    public VisitedPlace VisitedPlace { get; set; } = null!;
}