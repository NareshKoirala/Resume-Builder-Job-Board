using OpenAI.Chat;
using OpenAI.Models;
using Resume_Builder_MAUI.Model;
using Resume_Builder_MAUI.ViewModel;
using System.Data;
using System.Text;

namespace Resume_Builder_MAUI.Services
{
    static class OpenAIService
    {
        private static readonly ChatClient client = new ChatClient(
            apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"),
            model: "o4-mini-2025-04-16"
        );
        public static async Task JobDetailsAsync(string jobDesc, string applyLink)
        {
            GenJobsModel job = new GenJobsModel();

            if (jobDesc == null)
                return;

            string prompt = PromptViewModel.GetJobDetails(
                jobDesc ?? string.Empty
            );

            if (string.IsNullOrWhiteSpace(prompt))
                return;

            var messages = new[]
            {
                ChatMessage.CreateUserMessage(prompt)
            };

            try
            {
                var response = await client.CompleteChatAsync(messages);
                string reply = response.Value.Content[0].Text;
                var lines = reply.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length >= 2)
                {
                    job.JobName = lines[1].Replace("Title:", "").Trim();
                    job?.Jobs?.companyName = lines[0].Replace("Name:", "").Trim();
                    job?.Jobs?.jobDescription = jobDesc?.Trim();
                    job?.ApplyLink = applyLink?.Trim();

                    await PercentageAsync(job);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex.Message);
            }
        }
        public static async Task PercentageAsync(GenJobsModel? job)
        {
            if (job == null || job.Jobs == null)
                return;

            JobsModel jobsModel = job.Jobs;

            string prompt = await PromptViewModel.PercentageAsync(
                jobsModel.jobDescription ?? string.Empty,
                job.JobName ?? string.Empty,
                jobsModel?.companyName ?? string.Empty
            );

            if (prompt == "0")
                return;

            var messages = new[]
            {
                ChatMessage.CreateUserMessage(prompt)
            };

            try
            {
                var response = await client.CompleteChatAsync(messages);

                string reply = response.Value.Content[0].Text;

                job.jobPercentage = reply[1] + "" + reply[2] + "%, UserSkill match with Job Desc.";
                job.jobKeywords = reply.Substring(4).Trim();

                GlobalVarables.genJobs.Add(job);
            }
            catch (Exception ex)
            {
                job.jobPercentage = "Exception: " + ex.Message;
            }
        }

        public static async Task BuildResumePromptAsync(GenJobsModel job)
        {
            if (job == null || job.Jobs == null)
                return;

            JobsModel jobsModel = job.Jobs;

            string prompt = await PromptViewModel.BuildResumePromptAsync(
                job.JobName ?? string.Empty,
                jobsModel?.companyName ?? string.Empty,
                jobsModel?.jobDescription ?? string.Empty,
                job.jobKeywords ?? string.Empty
            );

            if (string.IsNullOrWhiteSpace(prompt))
                return;

            var messages = new[]
            {
                ChatMessage.CreateUserMessage(prompt)
            };
            try
            {
                var response = await client.CompleteChatAsync(messages);
                string reply = response.Value.Content[0].Text;
                var (resume, coverLetter) = ParseReply(reply);
                job.Resume = resume;
                job.CoverLetter = coverLetter;

                GlobalVarables.genJobs.Remove(job);
                GlobalVarables.genJobs.Add(job);
            }
            catch (Exception ex)
            {
                 throw new Exception("Exception: " + ex.Message);
            }
        }

        public static (ResumeModel Resume, CoverLetterModel CoverLetter) ParseReply(string reply)
        {
            var resume = new ResumeModel();
            var coverLetter = new CoverLetterModel();

            var sections = reply.Split(new[] { "TITLE:", "SUMMARY:", "SKILLS:", "PROJECTS:", "COVER LETTER:" }, StringSplitOptions.None);

            if (sections.Length < 5) throw new FormatException("Reply format is invalid or incomplete.");

            int index = sections.Length == 5 ? 0 : 1; // Adjust for the number of sections

            // TITLE Keyword
            resume.TitleKeyword = sections[index].Trim();
            index++;

            // SUMMARY
            resume.Summary = sections[index].Trim();
            index++;

            // SKILLS
            var skillsLines = sections[index].Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            index++;
            foreach (var line in skillsLines)
            {
                if (line.StartsWith("Programming Languages:", StringComparison.OrdinalIgnoreCase))
                    resume.ProgramingLanguage = line.Split(':')[1].Trim();
                else if (line.StartsWith("Frameworks & Libraries:", StringComparison.OrdinalIgnoreCase))
                    resume.Frameworks = line.Split(':')[1].Trim();
                else if (line.StartsWith("Relevant Keywords:", StringComparison.OrdinalIgnoreCase))
                    resume.RelevantKeywords = line.Split(':')[1].Trim();
            }

            // PROJECTS
            var projects = new List<ProjectsModel>();
            var projectChunks = sections[index].Split(new[] { "- **" }, StringSplitOptions.RemoveEmptyEntries);
            index++;

            foreach (var chunk in projectChunks)
            {
                var lines = chunk.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
                if (lines.Count < 3) continue;

                var project = new ProjectsModel();
                project.ProjectName = lines[0].Replace("**", "").Trim();
                project.Description = lines[1];

                var bullets = lines.Where(l => l.StartsWith("•")).ToList().Count < 2 ? lines.Where(l => l.StartsWith("-")).ToList() : lines.Where(l => l.StartsWith("•")).ToList();
                if (bullets.Count > 0) project.Bullet1 = bullets[0].Substring(1).Trim();
                if (bullets.Count > 1) project.Bullet2 = bullets[1].Substring(1).Trim();
                if (bullets.Count > 2) project.Bullet3 = bullets[2].Substring(1).Trim();

                projects.Add(project);
            }

            resume.Projects = projects;

            // COVER LETTER
            var linesCL = sections[index].Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder body1 = new(), body2 = new(), body3 = new();
            int state = 0;

            foreach (var line in linesCL)
            {
                string trimmed = line.Trim();

                if (trimmed.StartsWith("Title:", StringComparison.OrdinalIgnoreCase))
                {
                    coverLetter.Title = trimmed.Replace("Title:", "").Trim();
                    state = 0;
                }
                else if (trimmed.StartsWith("Body 1:", StringComparison.OrdinalIgnoreCase))
                {
                    state = 1;
                }
                else if (trimmed.StartsWith("Body 2:", StringComparison.OrdinalIgnoreCase))
                {
                    state = 2;
                }
                else if (trimmed.StartsWith("Body 3:", StringComparison.OrdinalIgnoreCase))
                {
                    state = 3;
                }
                else
                {
                    switch (state)
                    {
                        case 1: body1.AppendLine(trimmed); break;
                        case 2: body2.AppendLine(trimmed); break;
                        case 3: body3.AppendLine(trimmed); break;
                    }
                }
            }

            coverLetter.Body1 = body1.ToString().Trim();
            coverLetter.Body2 = body2.ToString().Trim();
            coverLetter.Body3 = body3.ToString().Trim().Replace("[Your Name]", GlobalVarables.userInfo.FirstName + " " + GlobalVarables.userInfo.LastName);
        
            return (resume, coverLetter);
        }
    }
}
