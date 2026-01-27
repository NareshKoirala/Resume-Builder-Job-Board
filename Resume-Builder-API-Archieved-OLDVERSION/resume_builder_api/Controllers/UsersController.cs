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
                    .Where(u => u.Email == authDto.Email)
                    .FirstOrDefault();

                return Ok(new { Message = "Authentication successful", userTemp });
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
            // Also remove related entries in other tables
            appDb.EducationEntries.RemoveRange(appDb.EducationEntries.Where(e => e.UserModelId == user.Id));
            appDb.WorkEntries.RemoveRange(appDb.WorkEntries.Where(w => w.UserModelId == user.Id));
            appDb.CertificateEntries.RemoveRange(appDb.CertificateEntries.Where(c => c.UserModelId == user.Id));
            appDb.SkillEntries.RemoveRange(appDb.SkillEntries.Where(s => s.UserModelId == user.Id));
            appDb.ProjectEntries.RemoveRange(appDb.ProjectEntries.Where(p => p.UserModelId == user.Id));
            appDb.Users.Remove(user);
            appDb.SaveChanges();

            // Return a success response
            return Ok(new { Message = "User deleted successfully" });

        }

        [HttpDelete("Delete/Education/{publicKey}")]
        public IActionResult DeleteEducation(string publicKey, EducationEntryDto education)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Find the education entry by ID
            var educationEntry = appDb.EducationEntries.FirstOrDefault(
                e => 
                    e.InstitutionName == education.InstitutionName &&
                    e.Date == education.Date &&
                    e.Location == education.Location &&
                    e.Details == education.Details &&
                    e.UserModelId == user.Id
                );
            // If education entry is not found, return a 404 Not Found response
            if (educationEntry == null)
            {
                return NotFound(new { Message = "Education entry not found" });
            }
            // Remove the education entry from the database
            appDb.EducationEntries.Remove(educationEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Education entry deleted successfully" });
        }

        [HttpDelete("Delete/Work/{publicKey}")]
        public IActionResult DeleteWork(string publicKey, WorkEntryDto work)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Find the work entry by ID
            var workEntry = appDb.WorkEntries.FirstOrDefault(
                w => 
                    w.Date == work.Date &&
                    w.CompanyName == work.CompanyName &&
                    w.Location == work.Location &&
                    w.Details == work.Details &&
                    w.UserModelId == user.Id
                );
            // If work entry is not found, return a 404 Not Found response
            if (workEntry == null)
            {
                return NotFound(new { Message = "Work entry not found" });
            }
            // Remove the work entry from the database
            appDb.WorkEntries.Remove(workEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Work entry deleted successfully" });
        }

        [HttpDelete("Delete/Certificate/{publicKey}")]
        public IActionResult DeleteCertificate(string publicKey, CertificateEntryDto certificate)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Find the certificate entry by ID
            var certificateEntry = appDb.CertificateEntries.FirstOrDefault(
                c => 
                    c.CertificateName == certificate.CertificateName &&
                    c.Details == certificate.Details &&
                    c.UserModelId == user.Id
                );
            // If certificate entry is not found, return a 404 Not Found response
            if (certificateEntry == null)
            {
                return NotFound(new { Message = "Certificate entry not found" });
            }
            // Remove the certificate entry from the database
            appDb.CertificateEntries.Remove(certificateEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Certificate entry deleted successfully" });
        }

        [HttpDelete("Delete/Skill/{publicKey}")]
        public IActionResult DeleteSkill(string publicKey, SkillsEntryDto skill)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Find the skill entry by ID
            var skillEntry = appDb.SkillEntries.FirstOrDefault(
                s => 
                    s.SkillName == skill.SkillName &&
                    s.UserModelId == user.Id
                );
            // If skill entry is not found, return a 404 Not Found response
            if (skillEntry == null)
            {
                return NotFound(new { Message = "Skill entry not found" });
            }
            // Remove the skill entry from the database
            appDb.SkillEntries.Remove(skillEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Skill entry deleted successfully" });
        }

        [HttpDelete("Delete/Project/{publicKey}")]
        public IActionResult DeleteProject(string publicKey, ProjectEntryDto project)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);
            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            // Find the project entry by ID
            var projectEntry = appDb.ProjectEntries.FirstOrDefault(
                p => 
                    p.ProjectName == project.ProjectName &&
                    p.Description == project.Description &&
                    p.UserModelId == user.Id
                );
            // If project entry is not found, return a 404 Not Found response
            if (projectEntry == null)
            {
                return NotFound(new { Message = "Project entry not found" });
            }
            // Remove the project entry from the database
            appDb.ProjectEntries.Remove(projectEntry);
            appDb.SaveChanges();
            // Return a success response
            return Ok(new { Message = "Project entry deleted successfully" });
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

        [HttpPut("Update/{publicKey}")]
        public IActionResult UpdateUserInfo(string publicKey, [FromBody] UserDto userInfoDto)
        {
            // Find the user by public key
            var user = appDb.Users.FirstOrDefault(u => u.PublicId == publicKey);

            // If user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Update the user's personal information
            user.FirstName = userInfoDto.FirstName ?? user.FirstName;
            user.LastName = userInfoDto.LastName ?? user.LastName;
            user.Email = userInfoDto.Email ?? user.Email;
            user.Mobile = userInfoDto.Mobile ?? user.Mobile;
            user.Location = userInfoDto.Location ?? user.Location;
            user.Province = userInfoDto.Province ?? user.Province;
            user.JobField = userInfoDto.JobField ?? user.JobField;
            user.PortfolioUrl = userInfoDto.PortfolioUrl ?? user.PortfolioUrl;
            user.LinkedInUrl = userInfoDto.LinkedInUrl ?? user.LinkedInUrl;
            user.UserSummary = userInfoDto.UserSummary ?? user.UserSummary;
            appDb.SaveChanges();

            // Return a success response
            return Ok(new { Message = "User information updated successfully", Response = user });
        }

    }
}
