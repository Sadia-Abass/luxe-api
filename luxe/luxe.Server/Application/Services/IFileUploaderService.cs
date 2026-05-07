using CloudinaryDotNet.Actions;

namespace luxe.Server.Application.Services
{
    public interface IFileUploaderService
    {
        Task<ImageUploadResult> UploadFileAsync(IFormFile file, string folder);
        Task<string> DeleteFileAsync(string publicId);
    }
}
