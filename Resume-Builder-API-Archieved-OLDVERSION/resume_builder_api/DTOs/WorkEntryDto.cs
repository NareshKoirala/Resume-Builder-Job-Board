using System.ComponentModel.DataAnnotations;

namespace resume_builder_api.DTOs
{
    public class WorkEntryDto
    {
        [Required]
        [StringLength(200)]
        public string? CompanyName { get; set; }

        [Required]
        [StringLength(200)]
        public string? Date { get; set; }

        [Required]
        [StringLength(100)]
        public string? Location { get; set; }

        [Required]
        [StringLength(2000)]
        public string? Details { get; set; }
    }
}
