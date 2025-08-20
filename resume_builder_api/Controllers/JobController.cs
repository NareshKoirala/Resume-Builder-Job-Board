using Microsoft.AspNetCore.Mvc;
using resume_builder_api.Models;
using resume_builder_api.DTOs;
using resume_builder_api.Services;

namespace resume_builder_api.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpGet("fetch-rcl-json")]
    public async Task<IActionResult> FetchResumeCoverLetterJsonAsync(JobDTO jobDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await HelperFunction.FetchUserModel(jobDto, _dbContext);


        // Convert JobDTO to JobModel
        var job = new JobModel
        {
            JobName = jobDto.JobName,
            JobDescription = jobDto.JobDescription
        };

        ReturnJobModel returnJob = new();
        returnJob.JobName = job.JobName;

        returnJob = await _openAIService.OpenAIServiceFunct(GenerateResume, user, job, returnJob);

        return Ok(new
        {
            jobName = returnJob.JobName,
            ResumeJson = returnJob.Resume,
            CoverLetterJson = returnJob.CoverLetter
        });
    }

    [HttpGet("get-matchKeys")]
    public async Task<IActionResult> GetMatchKeysAsync(JobDTO jobDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await HelperFunction.FetchUserModel(jobDto, _dbContext);

        // Convert JobDTO to JobModel
        var job = new JobModel
        {
            JobName = jobDto.JobName,
            JobDescription = jobDto.JobDescription
        };

        ReturnJobModel returnJob = new();
        returnJob.JobName = job.JobName;

        returnJob = await _openAIService.OpenAIServiceFunct(GeneratePrompt, user, job, returnJob);
        
        return Ok(new
        {
            jobName = returnJob.JobName,
            MatchPercentage = returnJob.JobPercentage,
            KeySkills = returnJob.JobKeywords
        });
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetJobAsync(JobDTO jobDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = await HelperFunction.FetchUserModel(jobDto, _dbContext);

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

        return Ok(returnJob);
    }

}
