
namespace Khdamatk.Server.Helper;

public class FileManagement(IWebHostEnvironment hostEnvironment) : IFileManagement
{
    private readonly IWebHostEnvironment hostEnvironment = hostEnvironment;

    private readonly string MediaPath = Path.Combine(hostEnvironment.WebRootPath, "Images");

    public Task DeleteFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> DownloadFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public async Task<int> UploadFileAsync(IFormFile file)
    {
        var media = new Media()
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Length
        };

        var filePath = Path.Combine(MediaPath, media.FileName);
        using (var stream = File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }


        return media.Id;
    }
}
