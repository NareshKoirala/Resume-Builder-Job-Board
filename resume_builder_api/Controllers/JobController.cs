using Microsoft.AspNetCore.Mvc;
using Desktop.Resume_Builder_API.resume_builder_api.DTOs;
using Desktop.Resume_Builder_API.resume_builder_api.Services;
using Desktop.Resume_Builder_API.resume_builder_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Desktop.Resume_Builder_API.resume_builder_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController : ControllerBase
{
    public readonly Func<UserModel, JobModel, string> GeneratePrompt = Prompt.Percentage;
    public readonly Func<UserModel, JobModel, string> GenerateResume = Prompt.Resume;
    public readonly OpenAIService _openAIService;
    public readonly AppDbContext _dbContext;

    public JobController(OpenAIService openAIService, AppDbContext dbContext)
    {
        _openAIService = openAIService;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> GetJobAsync(JobDTO jobDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        // Get user from database using publicID
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.PublicId == jobDto.PublicId);

        if (user == null)
        {
            return NotFound("User with PublicId not found. Highly likely DB issue.");
        }

        user.Certificates = await _dbContext.CertificateEntries.Where(c => c.UserModelId == user.Id).ToListAsync();
        user.Education = await _dbContext.EducationEntries.Where(e => e.UserModelId == user.Id).ToListAsync();
        user.Projects = await _dbContext.ProjectEntries.Where(p => p.UserModelId == user.Id).ToListAsync();
        user.Skills = await _dbContext.SkillEntries.Where(s => s.UserModelId == user.Id).ToListAsync();
        user.WorkExperience = await _dbContext.WorkEntries.Where(w => w.UserModelId == user.Id).ToListAsync();

        // Convert JobDTO to JobModel
        var job = new JobModel
        {
            JobName = jobDto.JobName,
            JobDescription = jobDto.JobDescription
        };

        ReturnJobModel returnJob = new();
        returnJob.JobName = job.JobName;

        returnJob = await _openAIService.OpenAIServiceFunct(GeneratePrompt, user, job, returnJob);
        returnJob = await _openAIService.OpenAIServiceFunct(GenerateResume, user, job, returnJob);

        return Ok(new { returnJob, user });
    }

}
