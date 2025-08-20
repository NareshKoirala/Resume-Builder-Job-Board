using System.ComponentModel.DataAnnotations;

namespace resume_builder_api.DTOs
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "User ID is required")]
        public required string PublicId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name can only contain letters and spaces")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name can only contain letters and spaces")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string Mobile { get; set; } = string.Empty;

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

    }
}