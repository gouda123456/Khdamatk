namespace Khdamatk.Server.Helper;

public interface IFileManagement
{
    Task<int> UploadFileAsync(IFormFile file);
    Task<Byte[]> DownloadFileAsync(string filePath);  
    Task DeleteFileAsync(string filePath);
}
