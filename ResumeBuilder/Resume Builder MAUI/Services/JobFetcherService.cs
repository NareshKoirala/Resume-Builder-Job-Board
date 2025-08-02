using Resume_Builder_MAUI.Model;
using System.Net;
using System.Text.RegularExpressions;

namespace Resume_Builder_MAUI.Services;

public class JobFetcher
{
    private static readonly HttpClient client = new();

    public static async Task<RemotiveJobsResponse?> RemotiveJobFetcherAsync(string url)
    {
        try
        {
            var response = await client.GetStringAsync(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RemotiveJobsResponse>(response);
        }
        catch (Exception ex)
        {
            GlobalVarables.ErrorMsg.Add($"JobFetcher | RemotiveJobFetcherAsync | Fetch Error: {ex.Message}");
            return null;
        }
    }

    public static async Task<JSearchJobsResponse?> JSearchJobFetcherAsync(string url)
    {
        try
        {
            try
            {
                string? apiKey = Environment.GetEnvironmentVariable("RAPIDAPI_KEY");
                if (string.IsNullOrEmpty(apiKey))
                {
                    GlobalVarables.ErrorMsg.Add("JobFetcher | JSearchJobFetcherAsync | API Key Missing");
                }
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "jsearch.p.rapidapi.com");
            }
            catch (Exception ex)
            {
                GlobalVarables.ErrorMsg.Add($"JobFetcher | JSearchJobFetcherAsync | API Key Error: {ex.Message}");
            }

            var response = await client.GetStringAsync(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JSearchJobsResponse>(response);
        }
        catch (Exception ex)
        {
            GlobalVarables.ErrorMsg.Add($"JobFetcher | JSearchJobFetcherAsync | Fetch Error: {ex.Message}");
            return null;
        }
    }

    public static async Task<JSearchJobsResponse?> JSearchDetailJobFetcherAsync(string url)
    {
        try
        {
            try
            {
                string? apiKey = Environment.GetEnvironmentVariable("RAPIDAPI_KEY");
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new Exception("RAPIDAPI_KEY environment variable is not set.");
                }
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "jsearch.p.rapidapi.com");
            }
            catch (Exception ex)
            {
                GlobalVarables.ErrorMsg.Add($"JobFetcher | JSearchDetailJobFetcherAsync | API Key Error: {ex.Message}");
            }

            var response = await client.GetStringAsync(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JSearchJobsResponse>(response);
        }
        catch (Exception ex)
        {
            GlobalVarables.ErrorMsg.Add($"JobFetcher | JSearchDetailJobFetcherAsync | Fetch Error: {ex.Message}");
            return null;
        }
    }

    public static async Task<List<JobsModel>> FetchAllJobsAsync(string remotiveUrl, string jsearchUrl)
    {
        var jobs = new List<JobsModel>();


        // Fetch JSearch jobs
        var jsearchResponse = await JSearchJobFetcherAsync(jsearchUrl);
        if (jsearchResponse?.data != null)
        {
            jobs.AddRange(jsearchResponse.data.Select(datum => new JobsModel
            {
                jobPublisher = "JSearch",
                jobId = datum.job_id,
                jobTitle = datum.job_title,
                companyName = datum.employer_name,
                companyLogo = datum.employer_logo,
                companyWebsite = datum.employer_website,
                jobEmploymentType = datum.job_employment_type,
                jobApplyLink = datum.job_apply_link,
                jobDescription = CleanHtml(datum.job_description),
                jobLocation = datum.job_location,
                jobCategory = null, // JSearch API does not provide this directly
                jobSalary = datum.job_salary?.ToString(),
                jobStatus = string.Empty,
                jobPostedAt = datum.job_posted_at
            }));
        }

        // Fetch Remotive jobs
        var remotiveResponse = await RemotiveJobFetcherAsync(remotiveUrl);
        if (remotiveResponse?.jobs != null)
        {
            jobs.AddRange(remotiveResponse.jobs.Select(job => new JobsModel
            {
                jobPublisher = "Remotive",
                jobId = job.id?.ToString(),
                jobTitle = job.title,
                companyName = job.company_name,
                companyLogo = job.company_logo ?? job.company_logo_url,
                companyWebsite = null, // Remotive API does not provide this
                jobEmploymentType = job.job_type,
                jobApplyLink = job.url,
                jobDescription = CleanHtml(job.description),
                jobLocation = job.candidate_required_location,
                jobCategory = job.category,
                jobSalary = job.salary,
                jobStatus = string.Empty,
                jobPostedAt = job.publication_date?.ToString("u")
            }));
        }

        return jobs;
    }

    public static string CleanHtml(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Step 1: Decode HTML entities
        string decoded = WebUtility.HtmlDecode(input);

        // Step 2: Remove HTML tags
        string noTags = Regex.Replace(decoded, "<.*?>", string.Empty);

        // Step 3: Trim each line
        var lines = noTags.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Trim();
        }

        // Step 4: Remove empty lines and normalize
        string cleaned = string.Join("\n", lines);
        cleaned = Regex.Replace(cleaned, @"^\s*$[\r\n]*", string.Empty, RegexOptions.Multiline);

        return cleaned.Trim();
    }
}

