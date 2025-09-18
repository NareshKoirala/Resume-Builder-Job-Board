using System.Text;
using System.Text.Json;
using resume_builder_api.Models;

namespace resume_builder_api.Services
{
    public class JobFetching
    {
        private readonly IConfiguration _configuration;

        private readonly string appId;  // Replace with your App ID
        private readonly string appKey; // Replace with your App Key

        public JobFetching(IConfiguration iconfiguration) {
            _configuration = iconfiguration;

            appId = _configuration["ConnectionStrings:appId"];  // Replace with your App ID
            appKey = _configuration["ConnectionStrings:appKey"]; // Replace with your App Key
        }

        public async Task<List<JobBoardReturn>?> AdzunaJobFetch(JobBoard jobBoard)
        {
            var results = new List<JobBoardReturn>();

            const string resultsPerPage = "20";
            const string sortBy = "date";


            var finalUrl = new StringBuilder(
                $"https://api.adzuna.com/v1/api/jobs/{jobBoard.country}/search/{jobBoard.page}?app_id={appId}&app_key={appKey}&results_per_page={resultsPerPage}&sort_by={sortBy}"
            );

            // Build query from JobBoard properties
            foreach (var prop in typeof(JobBoard).GetProperties())
            {
                if (prop.Name is nameof(JobBoard.page) or nameof(JobBoard.country))
                    continue;

                var value = prop.GetValue(jobBoard);
                if (value is null) continue;

                if (value is List<string> list && list.Count > 0)
                {
                    finalUrl.Append($"&{prop.Name}={Uri.EscapeDataString(string.Join(" ", list))}");
                }
                else
                {
                    finalUrl.Append($"&{prop.Name}={value}");
                }
            }

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            try
            {
                var response = await httpClient.GetAsync(finalUrl.ToString());
                response.EnsureSuccessStatusCode();

                var bytes = await response.Content.ReadAsByteArrayAsync();
                var content = Encoding.UTF8.GetString(bytes);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(content, options);

                if (jsonDict is null || !jsonDict.ContainsKey("results")) return results;

                var resultsElement = (JsonElement)jsonDict["results"];
                foreach (var job in resultsElement.EnumerateArray())
                {
                    results.Add(new JobBoardReturn
                    {
                        id = job.GetProperty("id").GetString() ?? string.Empty,
                        title = job.GetProperty("title").GetString() ?? string.Empty,
                        company = job.GetProperty("company").GetProperty("display_name").GetString() ?? string.Empty,
                        location = string.Join(" ", job.GetProperty("location").GetProperty("area").EnumerateArray().Select(x => x.GetString())),
                        description = job.GetProperty("description").GetString() ?? string.Empty,
                        salaryRange = job.GetProperty("salary_is_predicted").GetString() == "0" ? "Unavailable" : job.GetProperty("salary_is_predicted").GetString() ?? "Unavailable"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to fetch jobs: {ex.Message}");
                return null;
            }

            return results;
        }


    }
}
