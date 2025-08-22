using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using resume_builder_api.DTOs;
using resume_builder_api.Models;
using resume_builder_api.Services;

namespace resume_builder_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        public readonly Func<UserModel, JobDto, string> GenerateKeyword = Prompt.Percentage;
        public readonly Func<UserModel, JobDto, string> GenerateResume = Prompt.Resume;
        public readonly OpenAIService _openAIService;
        public readonly AppDbContext _dbContext;

        public JobsController(OpenAIService openAI, AppDbContext appDb)
        {
            _openAIService = openAI;
            _dbContext = appDb;
        }

        [HttpGet("/JSON/{publicKey}")]
        public async Task<IActionResult> ReturnJsonData(string publicKey, JobDto job)
        {
            if (job == null || publicKey == null) return BadRequest("Empty Feilds");
            var user = _dbContext.Users.FirstOrDefault(x => x.PublicId == publicKey);
            if (user == null) return BadRequest("Invalid Public ID");

            HelperFunction.FetchUser(user, _dbContext);

            var returnData = new ReturnJobModel();
            returnData.JobName = job.JobName;

            returnData = await _openAIService.OpenAIServiceFunct(GenerateKeyword, user, job, returnData);
            returnData = await _openAIService.OpenAIServiceFunct(GenerateResume, user, job, returnData);

            return Ok(new { Message = "Succesful Json format return", returnData });
        }

        [HttpGet("/PerKey/{publicKey}")]
        public async Task<IActionResult> ReturnPercentKeyword(string publicKey, JobDto job)
        {
            if (job == null || publicKey == null) return BadRequest("Empty Feilds");
            var user = _dbContext.Users.FirstOrDefault(x => x.PublicId == publicKey);
            if (user == null) return BadRequest("Invalid Public ID");

            HelperFunction.FetchUser(user, _dbContext);

            var returnData = new ReturnJobModel();
            returnData.JobName = job.JobName;

            returnData = await _openAIService.OpenAIServiceFunct(GenerateKeyword, user, job, returnData);

            return Ok(new {
                Message = "Success",
                Percent = returnData.JobPercentage,
                Keywords = returnData.JobKeywords
                });
        }

        [HttpGet("/Resume/Json/{publicKey}")]
        public async Task<IActionResult> ReturnResumeJsonData(string publicKey, JobDto job)
        {
            if (job == null || publicKey == null) return BadRequest("Empty Feilds");
            var user = _dbContext.Users.FirstOrDefault(x => x.PublicId == publicKey);
            if (user == null) return BadRequest("Invalid Public ID");

            HelperFunction.FetchUser(user, _dbContext);

            var returnData = new ReturnJobModel();
            returnData.JobName = job.JobName;

            returnData = await _openAIService.OpenAIServiceFunct(GenerateResume, user, job, returnData);

            return Ok(new { Message = "Succesful Json format return", returnData.Resume });
        }

        [HttpGet("/CoverLetter/Json/{publicKey}")]
        public async Task<IActionResult> ReturnCoverLetterJsonData(string publicKey, JobDto job)
        {
            if (job == null || publicKey == null) return BadRequest("Empty Feilds");
            var user = _dbContext.Users.FirstOrDefault(x => x.PublicId == publicKey);
            if (user == null) return BadRequest("Invalid Public ID");

            HelperFunction.FetchUser(user, _dbContext);

            var returnData = new ReturnJobModel();
            returnData.JobName = job.JobName;

            returnData = await _openAIService.OpenAIServiceFunct(GenerateResume, user, job, returnData);

            return Ok(new { Message = "Succesful Json format return", returnData.CoverLetter });
        }
    }
}
