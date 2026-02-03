namespace Khdamatk.Server.Data.Entities.Identity;
/*
 * (token , expireat) done
 * (create , used , revokedat)
 * (User)
 */
public class RefreshTokens
{
    //Core
    [Key]
    public int Id { get; set; }  //index
    [Required]
    public string Token { get; set; } = null!; 
    [Required]
    public DateTime ExpireAt { get; set; }


    //Date Times
    [Required]
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    
    public DateTime? UsedAt { get; set; }
    [DefaultValue(false)]
    public bool IsUsed { get; set; } = false;   //Default value is fasle
    
    public DateTime? RevokedAt { get; set; }
    

    public bool IsDeleted { get; set; } = false;

    public bool IsActive =>
        ExpireAt > DateTime.UtcNow &&
        !IsDeleted &&
        !IsUsed &&
        !RevokedAt.HasValue;

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
