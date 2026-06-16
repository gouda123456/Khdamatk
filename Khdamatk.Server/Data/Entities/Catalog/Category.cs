namespace Khdamatk.Server.Data.Entities.Catalog;

//Done: Category Entity Implementation
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required , Length(2,15)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public string Icon { get; set; }

    public bool IsActive { get; set; }

    public int ServiceCount { get; set; }
    public virtual ICollection<Service> Services { get; set; } = [];
}
