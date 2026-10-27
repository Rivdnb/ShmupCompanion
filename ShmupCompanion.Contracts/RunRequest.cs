using System.ComponentModel.DataAnnotations;

namespace ShmupCompanion.Contracts
{
    public class RunRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set;} = string.Empty;
    }
}