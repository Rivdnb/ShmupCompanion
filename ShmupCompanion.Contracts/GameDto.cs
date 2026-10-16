namespace ShmupCompanion.Contracts.Game
{
     public record class GameDto(
        int Id,
        string Name,
        string Developer,
        int ReleaseYear
    );
}