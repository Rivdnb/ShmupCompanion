namespace ShmupCompanion.Api
{
    public class Game
    {
        public required string Name { get; set; }

        public int Id { get; set; }

        public required string Developer { get; set; }

        public int ReleaseYear { get; set; }
    }
}
