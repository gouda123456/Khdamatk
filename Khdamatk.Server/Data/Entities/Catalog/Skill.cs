namespace Khdamatk.Server.Data.Entities.Catalog;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static List<Skill> Data = new List<Skill>
    {
        new () { Name = "web Development" },
        new () { Name = "Graphic Design" },
        new () { Name = "Content Writing" },
        new () { Name = "Digital Marketing" },
        new () { Name = "Data Analysis" },
        new () { Name = "App Development" },
        new () { Name = "Project Management" },
        new () { Name = "User Experience (UX) Design" },
        new () { Name = "User Interface (UI) Design" },
        new () { Name = "Translation" },
        new () { Name = "Video Editing" },
        new () { Name = "Photography" },
        new () { Name = "Voice Over" },
        new () { Name = "Social Media Management" },
        new () { Name = "Search Engine Optimization (SEO)" },
        new () { Name = "Copywriting" },
        new () { Name = "Virtual Assistance" },
        new () { Name = "Financial Consulting" },
        new () { Name = "Legal Consulting" },
        new () { Name = "Health and Wellness Coaching" },
        new () { Name = "Language Tutoring" },
        new () { Name = "Music Production" },
        new () { Name = "Animation" },
        new () { Name = "3D Modeling" },
        new () { Name = "Cybersecurity" },
        new () { Name = "Cloud Computing" },
        new () { Name = "Artificial Intelligence (AI)" },
        new () { Name = "Machine Learning" },
        new () { Name = "Blockchain Development" },
        new () { Name = "Game Development" },
        new () { Name = "Augmented Reality (AR) Development" },
        new () { Name = "Virtual Reality (VR) Development" },
        new () { Name = "Internet of Things (IoT) Development" },
        new () { Name = "Data Science" },
        new () { Name = "Big Data Analysis" },
        new () { Name = "Cloud Storage Solutions" },
        new () { Name = "Network Administration" },
        new () { Name = "Technical Support" },
        new () { Name = "IT Consulting" },
        new () { Name = "Software Development" },
    };
}
