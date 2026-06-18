namespace Khdamatk.Server.Data.Entities;

public static class DataSeeder
{

    //Catalog
    public static List<Category> Categories { get; set; } = Category.Data(4);
    public static List<Skill> Skills { get; set; } = Skill.Data;
    public static List<Service> Services { get; set; } = Service.Data;
    public static List<DeliveredJobFile> DeliveredJobFiles { get; set; } = DeliveredJobFile.Data(4);

    



    public static List<JobPost> JobPosts()
    {
        var jobPosts = new List<JobPost>()
        {
            new JobPost()
            {
                BudgetMax = 100,
                BudgetMin = 70,
                Category = Categories.FirstOrDefault(),
                CreatedAt = DateTime.Now,

            }
        };

        return jobPosts;
    } 
}
