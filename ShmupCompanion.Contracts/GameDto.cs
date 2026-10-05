namespace ShmupCompanion.Contracts.Games;

public record GameDto(
    int Id,
    string Name,
    string Developer,
    int ReleaseYear
);
