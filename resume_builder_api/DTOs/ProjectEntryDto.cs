using System.ComponentModel.DataAnnotations;

namespace resume_builder_api.DTOs
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
