using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderApi.DTOs
{
    public class CreateWorkEntryDto
    {
        [Required]
        [StringLength(200)]
        public string Company { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        public List<string>? Responsibilities { get; set; }

        public List<string>? Achievements { get; set; }

        [StringLength(50)]
        public string? EmploymentType { get; set; }
    }

    public class UpdateWorkEntryDto
    {
        [StringLength(200)]
        public string? Company { get; set; }

        [StringLength(100)]
        public string? Position { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        public List<string>? Responsibilities { get; set; }

        public List<string>? Achievements { get; set; }

        [StringLength(50)]
        public string? EmploymentType { get; set; }
    }

    public class WorkEntryResponseDto
    {
        public Guid Id { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public List<string>? Responsibilities { get; set; }
        public List<string>? Achievements { get; set; }
        public string? EmploymentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
