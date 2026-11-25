namespace ShmupCompanion.Api.Interfaces
{
    public interface INoteImageManagingService
    {
        public List<int> ParseHtmlImageIds(int noteId);
        public void UpdateNoteImages(int noteId);
    }
}
