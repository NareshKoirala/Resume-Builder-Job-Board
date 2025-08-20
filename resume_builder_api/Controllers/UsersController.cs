using Desktop.Resume_Builder_API.resume_builder_api.Models;
using Desktop.Resume_Builder_API.resume_builder_api.Services;
using Microsoft.AspNetCore.Mvc;
using Desktop.Resume_Builder_API.resume_builder_api.DTOs;
using resume_builder_api.DTOs;

namespace resume_builder_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController (IConfiguration configuration, AppDbContext appDb) : ControllerBase
    {
        [HttpPost("auth")]
        public IActionResult Authenticate(AuthDto authDto)
        {
            // Replace with your actual API key validation logic
            if (authDto.apiKey == configuration.GetConnectionString("ADMIN_PASS"))
            {
                // Check if the user already exists
                var existingUser = appDb.Users
                    .FirstOrDefault(u => u.Email == authDto.Email || u.Mobile == authDto.Mobile);

                if (existingUser != null)
                {
                    // If the user already exists, return a message
                    return Ok(new { Message = "User already exists", Response = existingUser });
                }

                appDb.Users.Add(new UserModel
                {
                    FirstName = authDto.FirstName,
                    LastName = authDto.LastName,
                    Email = authDto.Email,
                    Mobile = authDto.Mobile,
                    Location = "User_Location (City)",
                    Province = "User_Province (State)",
                    JobField = "User_Job_Field (Software Developer)",
                    PortfolioUrl = "https://user-portfolio.com",
                    LinkedInUrl = "https://linkedin.com/in/user-profile",
                    UserSummary = "User Summary (A brief description of the user)",
                    Education = new List<EducationEntry>() {
                        new EducationEntry
                        {
                            InstitutionName = "Institution_Name",
                            Date = "start and end year (2023-2025)",
                            Location = "Location of institution",
                            Details = "Description of the degree or achievements with course name and what you learned."
                        }
                    },
                    WorkExperience = new List<WorkEntry>() {
                        new WorkEntry
                        {
                            CompanyName = "Company_Name",
                            Date = "start and end year (2023-2025)",
                            Location = "Location of company",
                            Details = "Description of the job role, responsibilities, and achievements."
                        }
                    },
                    Certificates = new List<CertificateEntry>() {
                        new CertificateEntry
                        {
                            CertificateName = "Certificate_Name",
                            Details = "Description of the certificate and what it entails."
                        }
                    },
                    Skills = new List<SkillEntry>() {
                        new SkillEntry
                        {
                            SkillName = "Skill_Name"
                        }
                    },
                    Projects = new List<ProjectEntry>() {
                        new ProjectEntry
                        {
                            ProjectName = "Project_Name",
                            Description = "Description of the project, technologies used, and your role in it."
                        }
                    }
                });

                appDb.SaveChanges();

                // Return a success response
                var userTemp = appDb.Users
                    .Select(u => u.Email == authDto.Email)
                    .FirstOrDefault();

                return Ok(new { Message = "Authentication successful", Response = userTemp });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid API key" });
            }
        }
    }
}
