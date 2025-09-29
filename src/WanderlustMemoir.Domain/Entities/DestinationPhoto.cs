using WanderlustMemoir.Domain.Common;

namespace WanderlustMemoir.Domain.Entities;

public class DestinationPhoto : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public string? Description { get; set; }
    public DateTime? DateTaken { get; set; }
    public byte[] ImageData { get; set; } = Array.Empty<byte>(); // BLOB data
    public string ContentType { get; set; } = string.Empty; // MIME type
    public long FileSize { get; set; }
    public int DestinationId { get; set; }
    public Destination Destination { get; set; } = null!;
}