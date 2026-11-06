namespace ShmupCompanion.Api.Interfaces
{
    public interface IImageStorageService
    {
        Task<string> SaveAsync(
            IFormFile file,
            CancellationToken cancellationToken = default);
        Task DeleteAsync(string filePath);
    }
}
