using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using resume_builder_api.DTOs;
using resume_builder_api.Models;
using resume_builder_api.Services;

namespace resume_builder_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        public readonly Func<UserModel, JobDto, string> GenerateKeyword = Prompt.Percentage;
        public readonly Func<UserModel, JobDto, string> GenerateResume = Prompt.Resume;
        public readonly OpenAIService _openAIService;
        public readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public JobsController(OpenAIService openAI, AppDbContext appDb, IWebHostEnvironment env)
        {
            _openAIService = openAI;
            _dbContext = appDb;
            _env = env;
        }

        [HttpPost("/JSON/{publicKey}")]
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

        [HttpPost("/PerKey/{publicKey}")]
        public async Task<IActionResult> ReturnPercentKeyword(string publicKey, JobDto job)
        {
            if (job == null || publicKey == null) return BadRequest("Empty Feilds");
            var user = _dbContext.Users.FirstOrDefault(x => x.PublicId == publicKey);
            if (user == null) return BadRequest("Invalid Public ID");

            HelperFunction.FetchUser(user, _dbContext);

            var returnData = new ReturnJobModel();
            returnData.JobName = job.JobName;

            returnData = await _openAIService.OpenAIServiceFunct(GenerateKeyword, user, job, returnData);

            return Ok(new
            {
                Message = "Success",
                Percent = returnData.JobPercentage,
                Keywords = returnData.JobKeywords
            });
        }

        [HttpPost("/Resume/Json/{publicKey}")]
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

        [HttpPost("/CoverLetter/Json/{publicKey}")]
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

        [HttpPost("/Resume/Docx/{publicKey}")]
        public async Task<IActionResult> ReturnResumeDocx(string publicKey, JobDto job)
        {
            if (job == null || publicKey == null) return BadRequest("Empty Feilds");
            var user = _dbContext.Users.FirstOrDefault(x => x.PublicId == publicKey);
            if (user == null) return BadRequest("Invalid Public ID");

            HelperFunction.FetchUser(user, _dbContext);

            var returnData = new ReturnJobModel();
            returnData.JobName = job.JobName;

            returnData = await _openAIService.OpenAIServiceFunct(GenerateResume, user, job, returnData);


            // Create a memory stream (so we don't mess with the original template file)
            using (MemoryStream memStream = new MemoryStream())
            {
                var templatePath = Path.Combine(_env.ContentRootPath, "Templates", "Resume", "Template_1.docx");
                
                // Copy template into memory
                using (FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memStream);
                }

                // Reset position to beginning
                memStream.Position = 0;

                // Open WordprocessingDocument in editable mode
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memStream, true))
                {
                    Body body = wordDoc.MainDocumentPart.Document.Body;

                    // Call your function that formats the document
                    ResumeDocx.FixValueUpdate(body, user, returnData.Resume);

                    // Save changes
                    wordDoc.MainDocumentPart.Document.Save();
                }

                return File(
                        memStream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        $"{job.JobName}.docx"
                    );
            }
        }
    }
}
