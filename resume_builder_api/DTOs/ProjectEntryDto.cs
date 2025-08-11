using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderApi.DTOs
{
    public class CreateProjectEntryDto
    {
        [Required]
        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<string>? Technologies { get; set; }

        [Url]
        [StringLength(500)]
        public string? ProjectUrl { get; set; }

        [Url]
        [StringLength(500)]
        public string? RepositoryUrl { get; set; }

        [StringLength(100)]
        public string? Role { get; set; }
    }

    public class UpdateProjectEntryDto
    {
        [StringLength(200)]
        public string? ProjectName { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<string>? Technologies { get; set; }

        [Url]
        [StringLength(500)]
        public string? ProjectUrl { get; set; }

        [Url]
        [StringLength(500)]
        public string? RepositoryUrl { get; set; }

        [StringLength(100)]
        public string? Role { get; set; }
    }

    public class ProjectEntryResponseDto
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? Technologies { get; set; }
        public string? ProjectUrl { get; set; }
        public string? RepositoryUrl { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
