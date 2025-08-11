using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderApi.DTOs
{
    public class CreateEducationEntryDto
    {
        [Required]
        [StringLength(200)]
        public string? InstitutionName { get; set; }

        [Required]
        [StringLength(100)]
        public string? Degree { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(0.0, 4.0)]
        public decimal? Gpa { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }
}
