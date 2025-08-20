using Desktop.Resume_Builder_API.resume_builder_api.Models;
using ResumeBuilderApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Resume_Builder_API.resume_builder_api.DTOs;

public class UserRegisterDto : IValidatableObject
{
    [Required(ErrorMessage = "Location is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 100 characters")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Province is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Province must be between 2 and 50 characters")]
    public string Province { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job field is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Job field must be between 2 and 100 characters")]
    public string JobField { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Portfolio URL cannot exceed 500 characters")]
    public string PortfolioUrl { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "LinkedIn URL cannot exceed 500 characters")]
    public string LinkedInUrl { get; set; } = string.Empty;

    [StringLength(1500, ErrorMessage = "User summary cannot exceed 1500 characters")]
    public string UserSummary { get; set; } = string.Empty;

    [Required(ErrorMessage = "At least one education entry is required")]
    [MinLength(1, ErrorMessage = "At least one education entry is required")]
    public List<EducationEntryDto> Education { get; set; } = new();

    public List<WorkEntryDto> WorkExperience { get; set; } = new();

    public List<CertificateEntryDto> Certificates { get; set; } = new();

    [Required(ErrorMessage = "At least one skill is required")]
    [MinLength(1, ErrorMessage = "At least one skill is required")]
    public List<SkillsEntryDto> Skills { get; set; } = new();

    public List<ProjectEntryDto> Projects { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        // Validate education entries
        if (Education != null)
        {
            for (int i = 0; i < Education.Count; i++)
            {
                if (Education[i] == null)
                {
                    results.Add(new ValidationResult(
                        $"Education entry at index {i} cannot be null",
                        new[] { $"Education[{i}]" }));
                }
            }
        }

        // Validate skills entries
        if (Skills != null)
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                if (Skills[i] == null)
                {
                    results.Add(new ValidationResult(
                        $"Skill entry at index {i} cannot be null",
                        new[] { $"Skills[{i}]" }));
                }
            }
        }

        return results;
    }
}