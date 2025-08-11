using Microsoft.AspNetCore.Mvc;
using Desktop.Resume_Builder_API.resume_builder_api.DTOs;
using Desktop.Resume_Builder_API.resume_builder_api.Services;
using Desktop.Resume_Builder_API.resume_builder_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Desktop.Resume_Builder_API.resume_builder_api.Controllers;

[ApiController]
[Route("[controller]")]
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
            var user = HelperFunction.ReturnUserModel(userRegisterDto);

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

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateUserDTO updateUserDTO)
    {
        // var existingUser = await _dbContext.Users
        //     .FirstOrDefaultAsync(u => u.PublicId == updateUserDTO.PublicId);

        var existingUser = await _dbContext.Users
            .Include(u => u.Education)
            .Include(u => u.WorkExperience)
            .Include(u => u.Certificates)
            .Include(u => u.Skills)
            .Include(u => u.Projects)
            .FirstOrDefaultAsync(u => u.PublicId == updateUserDTO.PublicId);

        if (existingUser == null)
            return NotFound("User not found");

        // Map simple properties
        existingUser.FirstName = updateUserDTO.FirstName;
        existingUser.LastName = updateUserDTO.LastName;
        existingUser.Email = updateUserDTO.Email;
        existingUser.Mobile = updateUserDTO.Mobile;
        existingUser.Location = updateUserDTO.Location;
        existingUser.Province = updateUserDTO.Province;
        existingUser.JobField = updateUserDTO.JobField;
        existingUser.PortfolioUrl = updateUserDTO.PortfolioUrl;
        existingUser.LinkedInUrl = updateUserDTO.LinkedInUrl;
        existingUser.UserSummary = updateUserDTO.UserSummary;

        // Map nested collections
        existingUser.Education = [.. updateUserDTO.Education.Select(e => new EducationEntry
        {
            InstitutionName = e.InstitutionName,
            Date = e.Date,
            Location = e.Location,
            Details = e.Details
        })];

        existingUser.WorkExperience = [.. updateUserDTO.WorkExperience.Select(w => new WorkEntry
        {
            CompanyName = w.CompanyName,
            Location = w.Location,
            Details = w.Details
        })];

        existingUser.Certificates = [.. updateUserDTO.Certificates.Select(c => new CertificateEntry
        {
            CertificateName = c.CertificateName,
            Details = c.Details
        })];

        existingUser.Skills = [.. updateUserDTO.Skills.Select(s => new SkillEntry
        {
            SkillName = s.SkillName
        })];

        existingUser.Projects = [.. updateUserDTO.Projects.Select(p => new ProjectEntry
        {
            ProjectName = p.ProjectName,
            Description = p.Description
        })];

        await _dbContext.SaveChangesAsync();

        return Ok(new { Message = "User updated successfully" });
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string publicId)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.PublicId == publicId);

        if (user == null)
            return NotFound("User not found");

        _dbContext.Users.Remove(user);
        _dbContext.EducationEntries.RemoveRange(_dbContext.EducationEntries.Where(e => e.UserModelId == user.Id));
        _dbContext.WorkEntries.RemoveRange(_dbContext.WorkEntries.Where(w => w.UserModelId == user.Id));
        _dbContext.CertificateEntries.RemoveRange(_dbContext.CertificateEntries.Where(c => c.UserModelId == user.Id));
        _dbContext.SkillEntries.RemoveRange(_dbContext.SkillEntries.Where(s => s.UserModelId == user.Id));
        _dbContext.ProjectEntries.RemoveRange(_dbContext.ProjectEntries.Where(p => p.UserModelId == user.Id));
        await _dbContext.SaveChangesAsync();

        return Ok(new { Message = "User deleted successfully" });
    }
}
