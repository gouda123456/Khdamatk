namespace Khdamatk.Server.Data.Entities.Catalog;


public class ProviderSkill 
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(ServiceProviderProfile))]
    public string ServiceProviderProfileId { get; set; } = null!; 
    
    
    [Required]
    [StringLength(50, MinimumLength =2)]
    public string Name { get; set; } = string.Empty;


    [Required]
    public SkillExperienceLevel ExperienceLevel { get; set; } = SkillExperienceLevel.Beginner;


    public virtual ServiceProviderProfile ServiceProviderProfile { get; set; } = null!;
}



/// <summary>
/// يمثل مستوى الخبرة لمقدم الخدمة في مهارة معينة.
/// </summary>
public enum SkillExperienceLevel
{
    // عند تعيين القيمة 0، فإنها تصبح القيمة الافتراضية
    None = 0,

    // يفضل البدء من 1 لسهولة التحقق من الصحة (Validation)
    Beginner = 1,
    Novice = 2,
    Intermediate = 3,
    Advanced = 4,
    Expert = 5
}