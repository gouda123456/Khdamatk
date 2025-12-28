namespace Khdamatk.Server.Data.Entities.Catalog;

public record Media
{
    [Key]
    public int Id { get; init; }

    public string? FileName { get; init; }
    public string? ContentType { get; init; }
    public long Size { get; init; }

    public string FullPath => $"{ConstFilePathes.ImagesPath}{Id}";

    
}