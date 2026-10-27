using System.ComponentModel.DataAnnotations;

namespace ShmupCompanion.Api.Models
{
    public class Game
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(50)]
        public required string Developer { get; set; }

        public required int ReleaseYear { get; set; }

        public List<Run>? Runs { get; set; }
    }
}
