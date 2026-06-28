namespace Khdamatk.Server.Data.Entities.Catalog;

//Done: Category Entity Implementation
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required , Length(2,15)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public string Icon { get; set; } = "fa-solid fa-arrow-up-from-water-pump";

    public bool IsActive { get; set; } = true;

    public int ServiceCount => Services?.Count ?? 0;

    
    public virtual ICollection<Service>? Services { get; set; } = [];

    public static List<Category> Data(int minId)
    {
        var list = new List<Category>();

        for (int i = minId; i < minId +5; i++)
        {
            list.Add(new Category
            {
                Name = $"Category {i}",
                Description = $"Description for category {i}"
            });
        }

        return list;
    }

}
