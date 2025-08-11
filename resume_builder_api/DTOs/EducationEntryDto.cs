using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderApi.DTOs
{
    public class EducationEntryDto
    {
        [Required]
        [StringLength(200)]
        public string? InstitutionName { get; set; }

        [Required]
        [StringLength(100)]
        public string? Date { get; set; } 

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(1000)]
        public string? Details { get; set; }
    }
}
