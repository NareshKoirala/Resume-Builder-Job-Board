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

            appId = _configuration["appId"];  // Replace with your App ID
            appKey = _configuration["appKey"]; // Replace with your App Key
        }

        public async Task<List<JobBoardReturn>?> AdzunaJobFetch(JobBoard jobBoard)
        {
            var results = new List<JobBoardReturn>();

            const string resultsPerPage = "30";
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

                if (value is null)
                    continue;

                if (value is List<string> list)
                {
                    if (list.Count > 0)
                    {
                        finalUrl.Append($"&{prop.Name}={Uri.EscapeDataString(string.Join(" ", list))}");
                    }
                    else
                    {
                        // Append empty query param instead of skipping or ToString()
                        finalUrl.Append($"&{prop.Name}=");
                    }
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
                    job.TryGetProperty("company", out var companyProp);
                    job.TryGetProperty("location", out var locationProp);
                    job.TryGetProperty("contract_time", out var contractTimeProp);

                    results.Add(new JobBoardReturn
                    {
                        id = job.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "",
                        title = job.TryGetProperty("title", out var titleProp) ? titleProp.GetString() ?? "" : "",
                        company = companyProp.ValueKind != JsonValueKind.Undefined && companyProp.TryGetProperty("display_name", out var displayProp)
                                    ? displayProp.GetString() ?? ""
                                    : "",
                        location = locationProp.ValueKind != JsonValueKind.Undefined
                                    ? string.Join(" ", locationProp.GetProperty("area").EnumerateArray().Select(x => x.GetString()))
                                    : "",
                        description = job.TryGetProperty("description", out var descProp) ? descProp.GetString() ?? "" : "",
                        contract_time = contractTimeProp.ValueKind == JsonValueKind.Undefined ? "Unavailable" : contractTimeProp.GetString() ?? "Unavailable",
                        redirect_url = job.TryGetProperty("redirect_url", out var urlProp) ? urlProp.GetString() ?? "" : ""
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
