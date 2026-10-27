namespace ShmupCompanion.Contracts
{
     public record GameDto(
        int Id,
        string Name,
        string Developer,
        int ReleaseYear
    );
}