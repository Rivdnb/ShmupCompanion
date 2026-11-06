namespace ShmupCompanion.Api.Models
{
    public class NoteImage
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public Note Note { get; set; } = null!;
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } = "";
        public string FilePath { get; set; } = "";

    }
}
