using Resume_Builder_MAUI.Model;
using Resume_Builder_MAUI.Services;

namespace Resume_Builder_MAUI.ViewModel;

public static class JobsViewModel
{
    private const string JSearchBaseUrl = "https://jsearch.p.rapidapi.com/search?";
    private const string RemotiveBaseUrl = "https://remotive.com/api/remote-jobs?";

    public static (string remotiveUrl, string jsearchUrl)? BuildJobsUrls()
    {
        if (GlobalVarables.userInfo == null)
        {
            GlobalVarables.ErrorMsg.Add("JobsViewModel | BuildJobsUrls | User information is not set.");
            return null;
        }

        var remotiveUrl = $"{RemotiveBaseUrl}category={GlobalVarables.userInfo.JobField}&limit=10";
        var jsearchUrl = $"{JSearchBaseUrl}query={GlobalVarables.userInfo.JobField}%20jobs%20in%20{GlobalVarables.userInfo.Location}&" +
                         $"country={GlobalVarables.userInfo.Location}&date_posted=all";

        return (remotiveUrl, jsearchUrl);
    }

    public static async Task<List<JobsModel>> FetchJobsAsync()
    {
        var urls = BuildJobsUrls();

        if (urls == null)
        {
            GlobalVarables.ErrorMsg.Add("JobsViewModel | FetchJobsAsync | Failed to build job URLs.");
            return [];
        }
        var jobs = await JobFetcher.FetchAllJobsAsync(remotiveUrl:urls.Value.remotiveUrl, jsearchUrl:urls.Value.jsearchUrl);

        if (jobs == null || jobs.Count == 0)
        {
            GlobalVarables.ErrorMsg.Add("JobsViewModel | FetchJobsAsync | No jobs found or an error occurred while fetching jobs.");
            return [];
        }
        return jobs;
    }

    public static void RemoveJobFromList(JobsModel job)
    {
        if (GlobalVarables.jobsInfo != null)
        {
            GlobalVarables.jobsInfo.Remove(job);
        }
        else
        {
            GlobalVarables.ErrorMsg.Add("JobsViewModel | RemoveJobFromList | GlobalVarables.jobsInfo is null.");
        }
    }
}

