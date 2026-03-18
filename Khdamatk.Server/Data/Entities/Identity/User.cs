using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Khdamatk.Server.Data.Entities.Identity;

public class User : IdentityUser<string>
{
    public User()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters")]
    public virtual string FullName { get; set; } = string.Empty;

    // بنستخدم override هنا لأن Email موجود أصلاً في IdentityUser
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public override string? Email { get; set; }

    public virtual DateTime? DateOfBirth { get; set; }

    [NotMapped] // الحقول المحسوبة مش محتاجة تتسيف في الداتا بيز
    public virtual int? Age
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

    [Required]
    public virtual string Role { get; set; } = "User"; // Freelancer, Client, Admin

    [Required]
    public virtual string Status { get; set; } = "Active"; // Active, Blocked, Pending
    public bool IsVerified => VerificationData !=null && VerificationData?.Status == VerificationStatus.Approved;

    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual bool IsTrustedByAdmin { get; set; } = false;

    [NotMapped]
    public virtual bool IsServiceProvider => ServiceProviderProfile != null;

    [ForeignKey(nameof(ProfilePicture))]
    public virtual int? ProfilePictureId { get; set; }

    // --- Navigation Properties (لازم كلها virtual) ---

    public virtual VerificationData? VerificationData { get; set; }

    public virtual Media? ProfilePicture { get; set; }

    public virtual ServiceProviderProfile? ServiceProviderProfile { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual ICollection<VerificationsCodes> VerificationsCodes { get; set; } = new List<VerificationsCodes>();

    public virtual ICollection<UserFavorites> UserFavorites { get; set; } = new List<UserFavorites>();

    public virtual ICollection<RefreshTokens> RefreshTokens { get; set; } = new List<RefreshTokens>();

    public virtual ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();

    [NotMapped]
    public virtual bool IsVerified => VerificationData?.Status == VerificationStatus.Approved;
}