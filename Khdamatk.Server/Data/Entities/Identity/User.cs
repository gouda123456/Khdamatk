namespace Khdamatk.Server.Data.Entities.Identity;
 

public class User : IdentityUser<string>
{
    public User()
    {
        Id = Guid.NewGuid().ToString();
    }
    public DateTime? DateOfBirth { get; set; }
    // 2025 - 1990 
    public int? Age
    {
        get
        {
            if (DateOfBirth == null) return null;
            var today = DateTime.UtcNow;
            int age = today.Year - DateOfBirth.Value.Year;
            if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    public virtual VerificationData? VerificationData { get; set; }

    public bool IsVerified => VerificationData?.Status == VerificationStatus.Approved;

    [ForeignKey(nameof(ProfilePicture))]
    public int? ProfilePictureId { get; set; }
    public Media? ProfilePicture { get; set; }

    public bool IsTrustedByAdmin { get; set; } = false;


    public bool IsServiceProvider { get { return ServiceProviderProfile != null; }  }


    
    public virtual ServiceProviderProfile? ServiceProviderProfile { get; set; }

    
    public virtual List<VerificationsCodes> VerificationsCodes { get; set; } = [];
    public virtual List<UserFavorites> UserFavorites { get; set; } = [];
    public virtual List<RefreshTokens> RefreshTokens { get; set; } = [];
}
