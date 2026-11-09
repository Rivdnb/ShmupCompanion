using ShmupCompanion.Contracts;

namespace ShmupCompanion.Api.Interfaces
{
    public interface IImageStorageService
    {
        public string GetFilePath(string fileName);
        Task<string> SaveAsync(
            IFormFile file,
            CancellationToken cancellationToken = default);
        Task DeleteAsync(string filePath);
    }
}
