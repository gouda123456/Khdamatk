using Bogus;
namespace Khdamatk.Server.Helper;


public static class DataSeeder
{



    public static List<User> GetUsers()
    {
        var users = new List<User>()
        { 
            new User()
            {
                Id = "1",
                UserName = "admin",
                Amount = 100,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Email = "Admin@Admin.com",
                NormalizedEmail = "Admin@Admin.com",
                CreatedAt = DateTime.UtcNow,
                IsTrustedByAdmin = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                SecurityStamp = Guid.NewGuid().ToString(),
                NormalizedUserName = "ADMIN",
                EmailConfirmed = true,
                FullName = "Admin User",
                

            }
        };

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());
        return userFaker.Generate(50); // توليد 50 مستخدم
    }


    //Catalog
    public static List<Category> GetCategories()
    {
        var categoryFaker = new Faker<Category>()
            .RuleFor(c => c.Id, f => f.Random.Int(1, 1000))
            .RuleFor(c => c.Name, f => f.PickRandom("Programming", "Graphic Design", "Content Writing", "Marketing", "Video Editing"))
            .RuleFor(c => c.Description, f => f.Lorem.Sentence());

        return categoryFaker.Generate(10); // توليد 10 تصنيفات
    }

    public static List<Skill> GetSkills()
    {
        var skillFaker = new Faker<Skill>()
            .RuleFor(s => s.Id, f => f.Random.Int(1, 1000))
            .RuleFor(s => s.Name, f => f.PickRandom("C#", "Angular", "SQL", "Photoshop", "SEO", "Python"));

        return skillFaker.Generate(20); // توليد 20 مهارة
    }


    //Identity
    public static List<Role> GetRoles() => new List<Role>
    {
        new Role { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
        new Role { Id = Guid.NewGuid().ToString(), Name = "Freelancer", NormalizedName = "FREELANCER" },
        new Role { Id = Guid.NewGuid().ToString(), Name = "Client", NormalizedName = "CLIENT" }
    };


}
