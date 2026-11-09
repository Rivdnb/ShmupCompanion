using ShmupCompanion.Api.Interfaces;
using ShmupCompanion.Contracts;

namespace ShmupCompanion.Api.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly string _uploadPath;

        public ImageStorageService(IWebHostEnvironment environment)
        {
            _uploadPath = Path.Combine(
                environment.ContentRootPath,"App_Data","uploads");

            Directory.CreateDirectory(_uploadPath);
        }

        public string GetFilePath(string fileName)
        {
            return Path.Combine(_uploadPath, fileName);
        }
        public async Task<string> SaveAsync(
            IFormFile image,
            CancellationToken cancellationToken = default)
        {
            string extension = Path.GetExtension(image.FileName);
            // todo: validate file extension (here or elsewhere?)
            string fileName = $"{Guid.NewGuid()}{extension}";

            string fullPath = Path.Combine(_uploadPath, fileName);

            await using var stream = new FileStream(
                fullPath,
                FileMode.CreateNew); // error handling?

            await image.CopyToAsync(stream, cancellationToken);

            return fileName;
        }

        public Task DeleteAsync(string filePath)
        {
            string fullPath = Path.Combine(_uploadPath, filePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }

}
