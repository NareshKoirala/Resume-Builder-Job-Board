using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderApi.DTOs
{
    public class SkillsEntryDto
    {
        [Required]
        [StringLength(2000)]
        public string? SkillName { get; set; }
    }
}
