using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderApi.DTOs
{
    public class ProjectEntryDto
    {
        [Required]
        [StringLength(200)]
        public string? ProjectName { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }
    }
}
