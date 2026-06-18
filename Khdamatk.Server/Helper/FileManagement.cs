
namespace Khdamatk.Server.Helper;

public static class FileManagement
{
    public static IWebHostEnvironment hostEnvironment;

    public static string MediaPath;

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

    public static async Task<Media> UploadFileAsync(this IFormFile file)
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


    public static List<Media> SyncFolderWithDatabase(List<string> existingFileNames)
    {
        var newMediaList = new List<Media>();

        // 1. قراءة جميع الملفات من مجلد الـ Uploads
        var directoryInfo = new DirectoryInfo(MediaPath);
        if (!directoryInfo.Exists) return newMediaList;

        var filesInFolder = directoryInfo.GetFiles();

        // 2. استثناء الملفات الموجودة مسبقاً في الداتا بيز
        var filesToProcess = filesInFolder
            .Where(f => !existingFileNames.Contains(f.Name))
            .ToList();

        // 3. تحويل الملفات الجديدة إلى كائنات Media
        foreach (var file in filesToProcess)
        {
            newMediaList.Add(new Media
            {
                FileName = file.Name,
                FileExtension = file.Extension,
                Size = file.Length,
                ContentType = GetContentType(file.Extension) // ميثود مساعدة لجلب النوع
            });
        }

        return newMediaList;
    }

    // ميثود بسيطة لتحديد الـ ContentType بناءً على الامتداد
    private static string GetContentType(string extension)
    {
        return extension.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            _ => "application/octet-stream",
        };
    }

}
