
namespace Khdamatk.Server.Helper;

public static class FileManagement
{
    public static IWebHostEnvironment hostEnvironment;

    private static string MediaPath;

    public static void enableFileManagement(IWebHostEnvironment env)
    {
        hostEnvironment = env;
        MediaPath = Path.Combine(hostEnvironment.WebRootPath, "Uploads");
        if (!Directory.Exists(MediaPath))
        {
            Directory.CreateDirectory(MediaPath);
        }
    }

    public static Task DeleteFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public static byte[] DownloadFileAsyncByteVersion(this Media media)
    {
        if (media == null)
            return Array.Empty<byte>();

        var filePath = Path.Combine(MediaPath, media.FileName);

        var fileBytes = File.ReadAllBytes(filePath);

        return fileBytes;
    }

    public static string DownloadFileAsyncPathVersion(this Media media)
    {
        return Path.Combine(MediaPath, media.FileName);
    }

    public static async Task<Media> UploadFileAsync(IFormFile file)
    {
        var media = new Media()
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Length,
            FileExtension = Path.GetExtension(file.FileName)
            
        };

        var filePath = Path.Combine(MediaPath, media.FileName);
        using (var stream = File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }


        return media;
    }
}
