namespace ShmupCompanion.Api.Models
{
    public class Game
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Developer { get; set; }
        public int ReleaseYear { get; set; }
    }
}
