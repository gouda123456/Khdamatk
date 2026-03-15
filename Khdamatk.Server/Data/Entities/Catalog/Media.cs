namespace Khdamatk.Server.Data.Entities.Catalog;

public class Media
{
    [Key]
    public int Id { get; init; }

    public string? FileName { get; init; }
    public string StoredFileName { get; set; } = string.Empty;

    public string? ContentType { get; init; }
    public string FileExtension { get; set; } = string.Empty;
    public long Size { get; init; }

    public string FullPath { get; init; }


}