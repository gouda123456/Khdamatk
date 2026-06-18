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

    public bool SiteName { get; set; }

    public bool ContactEmail { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public bool TwoFactorAuthentication { get; set; }

    public bool PasswordPolicyEnabled { get; set; }

    public bool MaintenanceMode { get; set; }

    public bool EmailNotifications { get; set; }

    public bool SmsNotifications { get; set; }
    public string Attachments { get; set; }
    public virtual ICollection<Service> Services { get; set; } = [];

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
