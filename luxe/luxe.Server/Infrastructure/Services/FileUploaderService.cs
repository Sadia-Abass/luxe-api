using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using luxe.Server.Application.Services;
using luxe.Server.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace luxe.Server.Infrastructure.Services
{
    public class FileUploaderService : IFileUploaderService
    {
        private readonly Cloudinary _cloudinary;

        public FileUploaderService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var account = new Account(
                cloudinarySettings.Value.CloudName,
                cloudinarySettings.Value.ApiKey,
                cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }



        public async Task<ImageUploadResult> UploadFileAsync(IFormFile file, string folder)
        {
            var uplaodResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParam = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = $"Luxe/{folder}",
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    
                }; 

                uplaodResult = await _cloudinary.UploadAsync(uploadParam);
            }

            return uplaodResult; 
        }


        public async Task<string> DeleteFileAsync(string publicId)
        {
            var deleteParam = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParam);

            return result.Result == "OK" ? "File deleted successfully." : "Failed to delete file.";
        }
    }
}
