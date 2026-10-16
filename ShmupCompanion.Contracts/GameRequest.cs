using System.ComponentModel.DataAnnotations;

namespace ShmupCompanion.Contracts.Game
{
    public class GameRequest
    {
        [Required]
        public string Name { get; set;} = string.Empty;
        [Required]
        public string Developer { get; set; } = string.Empty;
        [Range(1960, 2026, ErrorMessage = $"Year must be in range of 1960 to 2026.")]
        public int ReleaseYear { get; set; } = 0;
    }
}