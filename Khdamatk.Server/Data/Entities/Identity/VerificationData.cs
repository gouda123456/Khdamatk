namespace Khdamatk.Server.Data.Entities.Identity;

public class VerificationData
{
    [Key]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; }

    public User User { get; set; } = null!;


    public string NationalNumber { get; set; } = string.Empty;
    
    public string Country { get; set; } = string.Empty;
    
    public string City { get; set; } = string.Empty;


    public VerificationStatus Status { get; set; } = VerificationStatus.Pending;

}


public enum VerificationStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}