using System.ComponentModel.DataAnnotations;

namespace ShmupCompanion.Api.Models
{
    public class Note
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        public int RunId { get; set; }

        public Run Run { get; set; } = null!;

        public string Text { get; set; } = string.Empty;

    }
}
