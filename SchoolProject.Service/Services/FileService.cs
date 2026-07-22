using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services
{
    public class FileServiceL:IFileService
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public FileServiceL(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return "NoImage";

            var extension = Path.GetExtension(file.FileName).ToLower();

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".webp"
             };

            if (!allowedExtensions.Contains(extension))
                return "InvalidImage";

            if (file.Length > 5 * 1024 * 1024)
                return "ImageTooLarge";

            var imageName = $"{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(hostEnvironment.WebRootPath, folder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, imageName);

            try
            {
                using var stream = new FileStream(filePath, FileMode.Create);

                await file.CopyToAsync(stream);

                return $"{folder}/{imageName}";
            }
            catch
            {
                return "FailedToUploadImage";
            }
        }
    }
}
