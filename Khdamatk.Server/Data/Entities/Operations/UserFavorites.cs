namespace Khdamatk.Server.Data.Entities.Operations;


public class UserFavorites
{
    // UserId and ServiceId Composite Primary Key
    [ForeignKey(nameof(User))]
    public string UserID { get; set; } = null!;

    [ForeignKey(nameof(Service))]
    public int ServiceID { get; set; }

    
    public DateTime AddedTime { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
