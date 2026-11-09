namespace ShmupCompanion.Contracts
{
    public record NoteImageDto(
       int Id,
       int NoteId,
       string FileName,
       string ContentType,
       string FilePath
   );
}
