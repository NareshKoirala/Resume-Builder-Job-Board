using resume_builder_api.DTOs;
using resume_builder_api.Models;

namespace resume_builder_api.Services
{
    public static class Prompt
    {
        private static readonly string JsonFilePath = "Resources/prompt.json";
        private static readonly Lazy<PromptData> _data = new(LoadPromptData);

        public static string PercentPrompt => _data.Value.PercentPrompt ?? "";
        public static string ResumePrompt => _data.Value.ResumePrompt ?? "";

        private static PromptData LoadPromptData()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, JsonFilePath);
                if (!File.Exists(path))
                {
                    // Log error here if you have a logger
                    return new PromptData();
                }
                var json = File.ReadAllText(path);
                var promptData = System.Text.Json.JsonSerializer.Deserialize<PromptData>(json);
                if (promptData == null)
                {
                    // Log error here if you have a logger
                    return new PromptData();
                }
                return promptData;
            }
            catch
            {
                // Log error here if you have a logger
                return new PromptData();
            }
        }

        private class PromptData
        {
            public string? PercentPrompt { get; set; }
            public string? ResumePrompt { get; set; }
        }

        public static string Percentage(UserModel user, JobDto job)
        {
            var skills = string.Join(", ", user.Skills.Select(s => s.SkillName));

            var promptUpdate = PercentPrompt
                .Replace("[JobName]", job.JobName)
                .Replace("[JobDescription]", job.JobDescription)
                .Replace("[Skills]", skills);

            return promptUpdate;
        }

        public static string Resume(UserModel user, JobDto job)
        {
            var skills = string.Join(", ", user.Skills.Select(s => s.SkillName));
            var projects = string.Join(", ", user.Projects.Select(p => $"{p.ProjectName}: {p.Description}"));
            var experiences = string.Join(", ", user.WorkExperience.Select(e => $"{e.CompanyName} at {e.Location} with {e.Details} at {e.Date}"));
            var education = string.Join(", ", user.Education.Select(e => $"{e.Date} in {e.Details} from {e.InstitutionName}"));
            var certifications = string.Join(", ", user.Certificates.Select(c => $"{c.CertificateName} ({c.Details})"));

            var promptUpdate = ResumePrompt
                .Replace("[JobName]", job.JobName)
                .Replace("[JobDescription]", job.JobDescription)
                .Replace("[Skills]", skills)
                .Replace("[Projects]", projects)
                .Replace("[FirstName]", user.FirstName)
                .Replace("[LastName]", user.LastName)
                .Replace("[Email]", user.Email)
                .Replace("[Mobile]", user.Mobile)
                .Replace("[Location]", user.Location)
                .Replace("[Province]", user.Province)
                .Replace("[JobField]", user.JobField)
                .Replace("[PortfolioUrl]", user.PortfolioUrl)
                .Replace("[LinkedInUrl]", user.LinkedInUrl)
                .Replace("[Certificates]", certifications)
                .Replace("[WorkExperience]", experiences)
                .Replace("[Education]", education)
                .Replace("[UserSummary]", user.UserSummary);

            return promptUpdate;
        }

    }
}