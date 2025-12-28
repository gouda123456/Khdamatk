namespace Khdamatk.Server.Data.Entities.Catalog;

//Done: Category Entity Implementation
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required , Length(2,15)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public virtual ICollection<Service> Services { get; set; } = [];
}
