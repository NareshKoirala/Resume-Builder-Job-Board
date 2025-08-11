using System.ComponentModel.DataAnnotations;
using Desktop.Resume_Builder_API.resume_builder_api.Services;

namespace Desktop.Resume_Builder_API.resume_builder_api.DTOs
{
    public class JobDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Public ID is required")]
        public string PublicId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Job name must be between 2 and 100 characters")]
        public string JobName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job description is required")]
        public string JobDescription { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Get the DbContext from dependency injection
            var dbContext = validationContext.GetService<AppDbContext>();

            if (dbContext != null && !string.IsNullOrEmpty(PublicId))
            {
                // Check if the PublicId exists in the database
                var exists = dbContext.Users.Any(j => j.PublicId == PublicId); // Replace 'Jobs' with your actual table/DbSet name

                if (!exists)
                {
                    results.Add(new ValidationResult(
                        "The specified Public ID does not exist in the database. Be sure to [api/User/register] Register a new user and save the publicId value you get.",
                        new[] { nameof(PublicId) }));
                }
            }

            return results;
        }
    }
}