

namespace Resume_Builder_MAUI.ViewModel
{
    static class PromptViewModel
    {
        private static Dictionary<string, string> _prompts = new() 
        {
            { 
                "percentPrompt", "Based on the following job context, infer the key technical skills required and list 5–10 relevant keywords for the job. " +
                "Then compare the job's technical skills with the user's skills and return only a single percentage showing how well they match. " +
                "Do not explain anything. " +
                "Format output as: [e.g., 65] [comma-separated job-related keywords that user need to improve]. " +
                "Job Title: [Job Title] " +
                "Company: [Company Name] " +
                "Summary: [3-line job summary] " +
                "User Skills: [comma-separated user skills]"
            },
            { 
                "resumePrompt", "You are generating an resume and cover letter for [Job Title] in [Job Company] from the provided user data. " +
                "Only use the actual [User Skills] and [User Projects]. " +
                "Use this User Summary Info about the user, [User Summary]." +
                "Align the content with the job using phrasing and structure, but do NOT invent new skills or experience. " +
                "Use the [Job Description] and [Job Keywords] only to guide language and formatting. " +
                "Follow the structure exactly as shown below. " +
                "TITLE: keyword1 | keyword2 | keyword3 " +
                "(Choose the top 3 title-style keywords most relevant to the job, e.g., “Full Stack Developer | Web Developer | .NET Engineer”)" +
                "SUMMARY:  " +
                "Write a 3-sentence summary in XYZ format:  " +
                "- X = background/experience  " +
                "- Y = skills/projects or achievements  " +
                "- Z = how it fits this job role" +
                "SKILLS:  " +
                "Programming Languages: [Only from user skills that overlap with job needs. Structure it in single line string seprated by | ]" +
                "Frameworks & Libraries: [Same rule — must be user-provided. Structure it in single line string seprated by | ] " +
                "Relevant Keywords: [Only use keywords that are in both user input and job context — like “REST APIs” , “Cloud”, “Agile”.  Structure it in single line string seprated by | ]" +
                "PROJECTS:  " +
                "Select the top 3 most relevant projects. " +
                "For each:" +
                "- {Make sure project title starts with a -  **} **Project Title** " +
                "- 2–3 sentence summary in XYZ format" +
                " - • Bullet 1 – Main task/responsibility " +
                " - • Bullet 2 – Contribution/result " +
                " - • Bullet 3 – Any measurable outcome or tool used" +
                "COVER LETTER: " +
                "Title: Application for" +
                "Body 1: " +
                "Short introduction, reference the role, express interest" +
                "Body 2: " +
                "Summarize experience, match real skills/projects to the job description using appropriate language" +
                "Body 3: " +
                "Close with availability, enthusiasm, and fit for the team"
            },
            {
                "jobDetails",
                "Based on the following job description: '[JobDesc]', extract and return the job title and company name.\n" +
                "Use the following output format:\n" +
                "Name: {Company Name}\n" +
                "Title: {Job Title}"
            }

        };

        public static async Task<string> PercentageAsync(string jobDesc, string jobTitle, string companyName)
        {
            var user = await GlobalVarables.service.GetSkillsAsync();

            string userSkills = string.Join(", ", user.Select(x => x.SkillName));

            if (string.IsNullOrWhiteSpace(jobDesc) || string.IsNullOrWhiteSpace(jobTitle) || string.IsNullOrWhiteSpace(companyName) || string.IsNullOrEmpty(userSkills))
            {
                return "0";
            }


            return _prompts["percentPrompt"]
                .Replace("[3-line job summary]", jobDesc)
                .Replace("[Job Title]", jobTitle)
                .Replace("[Company Name]", companyName)
                .Replace("[comma-separated user skills]", userSkills);
        }


        public static async Task<string> BuildResumePromptAsync(string jobTitle, string companyName, string jobDesc, string jobKeywords)
        {
            var user = await GlobalVarables.service.GetSkillsAsync();
            var userSummary = GlobalVarables.userInfo.User.UserSummary;
            var projects = await GlobalVarables.service.GetProjectsAsync();

            if (user.Count == 0 || projects.Count == 0)
            {
                return string.Empty;
            }

            string userSkills = string.Join(", ", user.Select(x => x.SkillName));
            var projectBlocks = string.Join("\n\n", projects
                .Select(p =>
                    $"-{p.ProjectName}\n" +
                    $"  - {p.Description}\n"
                ));

            if (string.IsNullOrWhiteSpace(jobTitle) || string.IsNullOrWhiteSpace(companyName) ||
                string.IsNullOrWhiteSpace(jobDesc) || string.IsNullOrEmpty(userSkills))
            {
                return string.Empty;
            }

            return _prompts["resumePrompt"]
                .Replace("[Job Title]", jobTitle)
                .Replace("[Job Company]", companyName)
                .Replace("[Job Description]", jobDesc)
                .Replace("[User Skills]", userSkills)
                .Replace("[Job Keywords]", jobKeywords)
                .Replace("[User Projects]", projectBlocks)
                .Replace("[User Summary]", userSummary);
        }

        public static string GetJobDetails(string jobDesc)
        {
            if (string.IsNullOrWhiteSpace(jobDesc))
            {
                return string.Empty;
            }
            return _prompts["jobDetails"]
                .Replace("[JobDesc]", jobDesc);
        }
    }
}
