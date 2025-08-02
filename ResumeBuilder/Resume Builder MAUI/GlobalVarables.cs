using HtmlAgilityPack;
using Resume_Builder_MAUI.Model;
using Resume_Builder_MAUI.Services;
using Resume_Builder_MAUI.ViewModel;
using System.Text;

namespace Resume_Builder_MAUI
{
    static class GlobalVarables
    {
        public static bool ValidSubmission { get; set; } = false;
        public static string Paths(string name) { return Path.Combine(FileSystem.AppDataDirectory, name); }
        public static List<string> ErrorMsg { get; set; } = new();

        public static string stringErrorMsg
        {
            get
            {
                if (ErrorMsg.Count == 0)
                    return string.Empty;
                return string.Join(", ", ErrorMsg.Distinct());
            }
        }

        public static UserViewModel userInfo { get; set; } = new();

        public static List<JobsModel>? jobsInfo { get; set; } = new();

        public static DatabaseService service = new(Paths("user.db"));

        public static List<GenJobsModel> genJobs { get; set; } = new();

        public static string ExtractSummaryFromDescription(string jobDescription)
        {
            if (string.IsNullOrWhiteSpace(jobDescription))
                return string.Empty;

            // Try to find "Description" or "Summary"
            int index = jobDescription.IndexOf("Description", StringComparison.OrdinalIgnoreCase);
            if (index == -1)
                index = jobDescription.IndexOf("Summary", StringComparison.OrdinalIgnoreCase);

            if (index == -1)
                return string.Empty; // Neither found

            int periodCount = 0;
            StringBuilder result = new();

            for (int i = index; i < jobDescription.Length; i++)
            {
                char c = jobDescription[i];
                result.Append(c);

                if (c == '.')
                {
                    periodCount++;
                    if (periodCount == 3)
                        break;
                }
            }

            return result.ToString().Trim();
        }


        public static async Task<string> LinkedInJobDescriptionAsync(string url)
        {
            HttpClient httpClient = new HttpClient();

            // Set headers to mimic a browser
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36"
            );

            string linkHtml = await httpClient.GetStringAsync(url);

            HtmlDocument linkDoc = new HtmlDocument();

            linkDoc.LoadHtml(linkHtml);

            // Extract <div class="show-more-less-html__markup relative overflow-hidden">
            var jobNode = linkDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'show-more-less-html__markup')]");
            return jobNode?.InnerText?.Trim() ?? "Description not found.";
        }
    }
}
