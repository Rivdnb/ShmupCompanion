using System.ComponentModel.DataAnnotations;

namespace ShmupCompanion.Contracts
{
    public class NoteRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set;} = string.Empty;
        [StringLength(1000)]
        public string Text { get; set; } = string.Empty;
    }
}