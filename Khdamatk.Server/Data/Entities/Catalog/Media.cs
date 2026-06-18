namespace Khdamatk.Server.Data.Entities.Catalog;

public class Media
{
    [Key]
    public int Id { get; init; }

    public string FileName { get; init; }

    public string? ContentType { get; init; }
    public string FileExtension { get; set; } = string.Empty;
    public long Size { get; init; }
    public string FullPath => this.DownloadFileAsyncPathVersion();


    public static List<Media> Data(int minId)
    {
        var list = new List<Media>();

        for (int i = minId; i < minId + 5; i++)
        {
            list.Add(new Media
            {
                Id = i,
                FileName = $"File{i}.txt",
                ContentType = "text/plain",
                FileExtension = ".txt",
                Size = 1024 * i
            });
        }

        return list;
    }

}