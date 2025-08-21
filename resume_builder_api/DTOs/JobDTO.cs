using System.ComponentModel.DataAnnotations;
using resume_builder_api.Services;

namespace resume_builder_api.DTOs
{
    public class JobDto
    {
        [Required(ErrorMessage = "Job name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Job name must be between 2 and 100 characters")]
        public string JobName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job description is required")]
        public string JobDescription { get; set; } = string.Empty;
    }
}