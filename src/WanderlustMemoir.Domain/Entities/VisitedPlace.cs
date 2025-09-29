using WanderlustMemoir.Domain.Common;

namespace WanderlustMemoir.Domain.Entities;

public class VisitedPlace : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public List<Photo> Photos { get; set; } = new();
}