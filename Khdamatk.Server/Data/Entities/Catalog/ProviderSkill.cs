namespace Khdamatk.Server.Data.Entities.Catalog;


public class ProviderSkill 
{
    public string ServiceProviderProfileId { get; set; } = string.Empty;
    public int SkillId { get; set; }

    public SkillExperienceLevel MyLevel { get; set; } // خبرة الشخص الفعلية

    public virtual ServiceProviderProfile Profile { get; set; } = null!;
    public virtual Skill Skill { get; set; } = null!;
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