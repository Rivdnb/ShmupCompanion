namespace ShmupCompanion.Api.Interfaces
{
    public interface IImageStorageService
    {
        public string GetFilePath(string fileName);
        public Task<string> SaveAsync(
            IFormFile file,
            CancellationToken cancellationToken = default);
        public void Delete(string filePath);
    }
}
