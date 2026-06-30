using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Khdamatk.Server.Helper.Validations;

public class AllowedFileTypesAttribute : ValidationAttribute
{
    private static readonly Dictionary<string, List<byte[]>> AllowedFileSignatures = new()
    {
        // Images
        { ".jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },
        { ".jpg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },
        { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
        { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
        { ".bmp", new List<byte[]> { new byte[] { 0x42, 0x4D } } },
        { ".webp", new List<byte[]> { new byte[] { 0x52, 0x49, 0x46, 0x46 } } }, // Requires further check for WEBP, but RIFF is 1st
        
        // Videos
        { ".mp4", new List<byte[]> { new byte[] { 0x66, 0x74, 0x79, 0x70 } } }, // Usually offset by 4 bytes
        { ".avi", new List<byte[]> { new byte[] { 0x52, 0x49, 0x46, 0x46 } } },
        { ".mkv", new List<byte[]> { new byte[] { 0x1A, 0x45, 0xDF, 0xA3 } } },
        { ".mov", new List<byte[]> { new byte[] { 0x66, 0x74, 0x79, 0x70 } } }, // Similar to MP4
        { ".wmv", new List<byte[]> { new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11 } } },

        // Documents
        { ".pdf", new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } },
        
        // Old Office formats (doc, xls, ppt)
        { ".doc", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } } },
        { ".xls", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } } },
        { ".ppt", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } } },

        // New Office formats (docx, xlsx, pptx) - These are ZIP archives
        { ".docx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 }, new byte[] { 0x50, 0x4B, 0x05, 0x06 }, new byte[] { 0x50, 0x4B, 0x07, 0x08 } } },
        { ".xlsx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 }, new byte[] { 0x50, 0x4B, 0x05, 0x06 }, new byte[] { 0x50, 0x4B, 0x07, 0x08 } } },
        { ".pptx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 }, new byte[] { 0x50, 0x4B, 0x05, 0x06 }, new byte[] { 0x50, 0x4B, 0x07, 0x08 } } }
    };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var files = new List<IFormFile>();

        if (value is IFormFile file)
        {
            files.Add(file);
        }
        else if (value is IEnumerable<IFormFile> fileList)
        {
            files.AddRange(fileList);
        }
        else
        {
            return ValidationResult.Success; // Not a file
        }

        foreach (var f in files)
        {
            if (f.Length == 0) continue;

            var extension = Path.GetExtension(f.FileName).ToLowerInvariant();

            if (!AllowedFileSignatures.ContainsKey(extension))
            {
                return new ValidationResult($"Invalid file type for {f.FileName}. Only images, videos, PDF, and Office files are allowed.");
            }

            if (!IsValidFileSignature(f, extension))
            {
                return new ValidationResult($"File content of {f.FileName} does not match its extension. Fake extensions are not allowed.");
            }
        }

        return ValidationResult.Success;
    }

    private bool IsValidFileSignature(IFormFile file, string extension)
    {
        var signatures = AllowedFileSignatures[extension];
        var headerBytes = new byte[signatures.Max(m => m.Length) + 4]; // +4 to handle offset like MP4
        
        using (var reader = new BinaryReader(file.OpenReadStream()))
        {
            headerBytes = reader.ReadBytes(headerBytes.Length);
        }

        foreach (var signature in signatures)
        {
            // For MP4 and MOV formats, the signature "ftyp" usually starts at byte 4
            if (extension == ".mp4" || extension == ".mov")
            {
                if (headerBytes.Length >= 8 && headerBytes.Skip(4).Take(4).SequenceEqual(signature))
                {
                    return true;
                }
            }
            
            // For WEBP, RIFF is first 4 bytes, then WEBP is from byte 8 to 11
            if (extension == ".webp")
            {
                var webpSignature = new byte[] { 0x57, 0x45, 0x42, 0x50 }; // "WEBP"
                if (headerBytes.Length >= 12 && 
                    headerBytes.Take(4).SequenceEqual(signature) &&
                    headerBytes.Skip(8).Take(4).SequenceEqual(webpSignature))
                {
                    return true;
                }
            }

            if (headerBytes.Take(signature.Length).SequenceEqual(signature))
            {
                return true;
            }
        }

        return false;
    }
}
