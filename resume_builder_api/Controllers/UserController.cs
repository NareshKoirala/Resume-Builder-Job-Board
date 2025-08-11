using Microsoft.AspNetCore.Mvc;
using Desktop.Resume_Builder_API.resume_builder_api.DTOs;
using Desktop.Resume_Builder_API.resume_builder_api.Services;
using Desktop.Resume_Builder_API.resume_builder_api.Models;

namespace Desktop.Resume_Builder_API.resume_builder_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public UserController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            var educationEntries = userRegisterDto.Education.Select(e => new EducationEntry
            {
                InstitutionName = e.InstitutionName,
                Date = e.Date,
                Location = e.Location,
                Details = e.Details
            }).ToList();

            var workExperienceEntries = userRegisterDto.WorkExperience.Select(w => new WorkEntry
            {
                CompanyName = w.CompanyName,
                Location = w.Location,
                Details = w.Details
            }).ToList();

            var certificateEntries = userRegisterDto.Certificates.Select(c => new CertificateEntry
            {
                CertificateName = c.CertificateName,
                Details = c.Details
            }).ToList();

            var skillsEntries = userRegisterDto.Skills.Select(s => new SkillEntry
            {
                SkillName = s.SkillName
            }).ToList();

            var projectsEntries = userRegisterDto.Projects.Select(p => new ProjectEntry
            {
                ProjectName = p.ProjectName,
                Description = p.Description
            }).ToList();

            var user = new UserModel
            {
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Email = userRegisterDto.Email,
                Mobile = userRegisterDto.Mobile,
                Location = userRegisterDto.Location,
                Province = userRegisterDto.Province,
                JobField = userRegisterDto.JobField,
                PortfolioUrl = userRegisterDto.PortfolioUrl,
                LinkedInUrl = userRegisterDto.LinkedInUrl,
                UserSummary = userRegisterDto.UserSummary,
                Education = educationEntries,
                WorkExperience = workExperienceEntries,
                Certificates = certificateEntries,
                Skills = skillsEntries,
                Projects = projectsEntries
            };

            // Save user first to get the generated ID and PublicId
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Return user with database-generated PublicId
            return Ok(new { PublicId = user.PublicId, Message = "User registered successfully" });
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, "An error occurred while registering the user");
        }
    }
}
