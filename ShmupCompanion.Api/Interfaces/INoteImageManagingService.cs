namespace ShmupCompanion.Api.Interfaces
{
    public interface INoteImageManagingService
    {
        public List<int> ParseHtmlImageIds(string source);
        public Task UpdateNoteImagesAsync(int noteId);
    }
}
