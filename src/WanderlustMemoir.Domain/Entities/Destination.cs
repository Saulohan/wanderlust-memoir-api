using WanderlustMemoir.Domain.Common;

namespace WanderlustMemoir.Domain.Entities;

public class Destination : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsVisited { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
}

public enum Priority
{
    Low = 0,
    Medium = 1,
    High = 2
}