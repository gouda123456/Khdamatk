namespace Khdamatk.Server.Data.Entities.Catalog;

public class JobSkillRequirement
{
    public int JobPostId { get; set; }
    public int SkillId { get; set; }

    public SkillExperienceLevel RequiredLevel { get; set; } // الخبرة المطلوبة لهذه الوظيفة

    public virtual JobPost JobPost { get; set; } = null!;
    public virtual Skill Skill { get; set; } = null!;
}