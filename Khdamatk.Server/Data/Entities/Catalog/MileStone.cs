namespace Khdamatk.Server.Data.Entities.Catalog;

public class MileStone
{
    [Key]
    public int Id { get; set; }

    [Required , StringLength(150)]
    public string Title { get; set; } = string.Empty;
    [Required, StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public int StepNumber { get; set; } = 1;
    [Required]
    public bool IsCompleted { get; set; } = false;

    [Required]
    public decimal Price { get; set; }


    public static List<MileStone> Data(int minId)
    {
        var list = new List<MileStone>();

        for (int i = minId; i < minId + 5; i++)
        {
            list.Add(new MileStone
            {
                Id = i,
                Title = $"MileStone {i}",
                Description = $"Description for mile stone {i}",
                StepNumber = i,
                IsCompleted = false,
                Price = 100 + i
            });
        }

        return list;
    }
}
