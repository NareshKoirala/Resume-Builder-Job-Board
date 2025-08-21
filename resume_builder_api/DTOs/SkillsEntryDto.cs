using System.ComponentModel.DataAnnotations;

namespace resume_builder_api.DTOs
{
    public class SkillsEntryDto
    {
        [Required]
        [StringLength(2000)]
        public string? SkillName { get; set; }
    }
}
