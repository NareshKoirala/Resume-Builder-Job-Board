using Microsoft.AspNetCore.Mvc;
using resume_builder_api.DTOs;
using resume_builder_api.Models;
using resume_builder_api.Services;

namespace resume_builder_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IConfiguration configuration, AppDbContext appDb) : ControllerBase
    {
        [HttpPost("Auth")]
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

        [HttpGet("{publicKey}")]
        public IActionResult GetUserAll(string publicKey)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);

            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            user.Education = appDb.EducationEntries
                .Where(e => e.UserModelId == user.Id)
                .ToList();

            user.WorkExperience = appDb.WorkEntries
                .Where(w => w.UserModelId == user.Id)
                .ToList();

            user.Certificates = appDb.CertificateEntries
                .Where(c => c.UserModelId == user.Id)
                .ToList();

            user.Skills = appDb.SkillEntries
                .Where(s => s.UserModelId == user.Id)
                .ToList();

            user.Projects = appDb.ProjectEntries
                .Where(p => p.UserModelId == user.Id)
                .ToList();

            // Return the user details
            return Ok(user);
        }

        [HttpDelete("Delete/{publicKey}")]
        public IActionResult DeleteUser(string publicKey)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);

            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Remove the user from the database
            appDb.Users.Remove(user);
            appDb.SaveChanges();

            // Return a success response
            return Ok(new { Message = "User deleted successfully" });

        }

        [HttpPut("Update/Education/{publicKey}")]
        public IActionResult UpdateEducation(string publicKey, [FromBody] EducationEntryDto entryDto)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);

            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Add the new education entry to the user's education list
            var educationEntry = new EducationEntry()
            {
                UserModelId = user.Id,
                InstitutionName = entryDto.InstitutionName,
                Date = entryDto.Date,
                Location = entryDto.Location,
                Details = entryDto.Details
            };

            appDb.EducationEntries.Add(educationEntry);
            appDb.SaveChanges();

            // Return a success response
            return Ok(new { Message = "Education updated successfully", Response = educationEntry });
        }

        [HttpPut("Update/Work/{publicKey}")]
        public IActionResult UpdateWork(string publicKey, [FromBody] WorkEntryDto entryDto)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Add the new work entry to the user's work experience list
            var workEntry = new WorkEntry()
            {
                UserModelId = user.Id,
                CompanyName = entryDto.CompanyName,
                Date = entryDto.Date,
                Location = entryDto.Location,
                Details = entryDto.Details
            };
            appDb.WorkEntries.Add(workEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Work updated successfully", Response = workEntry });
        }

        [HttpPut("Update/Certificate/{publicKey}")]
        public IActionResult UpdateCertificate(string publicKey, [FromBody] CertificateEntryDto entryDto)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Add the new certificate entry to the user's certificates list
            var certificateEntry = new CertificateEntry()
            {
                UserModelId = user.Id,
                CertificateName = entryDto.CertificateName,
                Details = entryDto.Details
            };
            appDb.CertificateEntries.Add(certificateEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Certificate updated successfully", Response = certificateEntry });
        }

        [HttpPut("Update/Skill/{publicKey}")]
        public IActionResult UpdateSkill(string publicKey, [FromBody] SkillsEntryDto entryDto)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Add the new skill entry to the user's skills list
            var skillEntry = new SkillEntry()
            {
                UserModelId = user.Id,
                SkillName = entryDto.SkillName
            };
            appDb.SkillEntries.Add(skillEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Skill updated successfully", Response = skillEntry });
        }

        [HttpPut("Update/Project/{publicKey}")]
        public IActionResult UpdateProject(string publicKey, [FromBody] ProjectEntryDto entryDto)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Add the new project entry to the user's projects list
            var projectEntry = new ProjectEntry()
            {
                UserModelId = user.Id,
                ProjectName = entryDto.ProjectName,
                Description = entryDto.Description
            };
            appDb.ProjectEntries.Add(projectEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Project updated successfully", Response = projectEntry });
        }

    }
}
