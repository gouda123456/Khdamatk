namespace Khdamatk.Server.Data.Entities.Catalog;

public class JobSkillRequirement
{
    public int JobPostId { get; set; }
    public int SkillId { get; set; }

    public SkillExperienceLevel RequiredLevel { get; set; } // الخبرة المطلوبة لهذه الوظيفة

    public virtual JobPost JobPost { get; set; } = null!;
    public virtual Skill Skill { get; set; } = null!;

        public static List<JobSkillRequirement> Data(int minId)
        {
            var list = new List<JobSkillRequirement>();
    
            for (int i = minId; i < minId + 5; i+=2)
            {
                list.AddRange(new JobSkillRequirement
                {
                    JobPostId = i,
                    SkillId = i,
                    RequiredLevel = SkillExperienceLevel.Intermediate
                });
            }
    
            return list;
    }
}