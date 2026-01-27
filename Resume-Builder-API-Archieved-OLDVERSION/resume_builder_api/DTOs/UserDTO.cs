using System.ComponentModel.DataAnnotations;

namespace resume_builder_api.DTOs;

public class UserDto
{
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

    [Required(ErrorMessage = "Portfolio is required if not add N/A")]
    [StringLength(500, ErrorMessage = "Portfolio URL cannot exceed 500 characters")]
    public string PortfolioUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "LinkedIn URL is required if not add N/A")]
    [StringLength(500, ErrorMessage = "LinkedIn URL cannot exceed 500 characters")]
    public string LinkedInUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "User summary is required")]
    [StringLength(1500, ErrorMessage = "User summary cannot exceed 1500 characters")]
    public string UserSummary { get; set; } = string.Empty;
}