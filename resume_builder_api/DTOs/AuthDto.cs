using System.ComponentModel.DataAnnotations;

namespace resume_builder_api.DTOs
{
    public class AuthDto
    {
        [Required(ErrorMessage = "Pass is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Contact Naresh at (https://www.nareshkoirala.dev/contact) to get a pass.")]
        public string apiKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name can only contain letters and spaces")]
        public string FirstName {  get; set; } = string.Empty;

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

    }
}
