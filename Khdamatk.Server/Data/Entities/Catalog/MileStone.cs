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
}
