namespace ShmupCompanion.Api.Interfaces
{
    public interface INoteImageManagerService
    {
        public List<int> ParseHtmlImageIds(string source);
        public Task UpdateNoteImagesAsync(int noteId);
        public Task DeleteNoteImagesAsync(CancellationToken cancellationToken);
    }
}
