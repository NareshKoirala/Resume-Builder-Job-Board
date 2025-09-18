using Microsoft.AspNetCore.Mvc;
using resume_builder_api.Models;
using resume_builder_api.Services;

namespace resume_builder_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobBoardController : ControllerBase
    {
        private readonly JobFetching jobFetching;
        public JobBoardController(JobFetching job) 
        {
            jobFetching = job;
        }

        [HttpPost]
        public async Task<IActionResult> ReturnJobBoard(JobBoard jobDetail)
        {
            var returnData = await jobFetching.AdzunaJobFetch(jobDetail);

            return Ok(returnData);
        }
    }
}
