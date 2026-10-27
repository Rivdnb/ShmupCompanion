using System.ComponentModel.DataAnnotations;

namespace ShmupCompanion.Api.Models
{
    public class Run
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; } = null!;

        //public List<Note>? Notes { get; set; }
    }
}
