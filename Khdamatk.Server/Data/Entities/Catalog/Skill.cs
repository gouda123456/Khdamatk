namespace Khdamatk.Server.Data.Entities.Catalog;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static List<Skill> Data = new List<Skill>
    {
        new (){ Id = 1, Name = "web Development" },
        new () { Id = 2, Name = "Graphic Design" },
        new () { Id = 3, Name = "Content Writing" },
        new () { Id = 4, Name = "Digital Marketing" },
        new () { Id = 5, Name = "Data Analysis" },
        new () { Id = 6, Name = "App Development" },
        new () { Id = 7, Name = "Project Management" },
        new () { Id = 8, Name = "User Experience (UX) Design" },
        new () { Id = 9, Name = "User Interface (UI) Design" },
        new () { Id = 10, Name = "Translation" }
    };
}
