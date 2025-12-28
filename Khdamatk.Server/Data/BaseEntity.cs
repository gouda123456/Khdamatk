namespace Khdamatk.Server.Data;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Createdat { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? Updatedat { get; set; }
    public string? UpdatedBy { get; set; }

    public bool IsDelete { get; set; } = false;
}
