using OpenAI.Chat;
using Desktop.Resume_Builder_API.resume_builder_api.Models;
using System.Data;
using System.Text.RegularExpressions;

namespace Desktop.Resume_Builder_API.resume_builder_api.Services;

public class OpenAIService
{
    private readonly IConfiguration _configuration;

    private readonly ChatClient client;

    public OpenAIService(IConfiguration configuration)
    {
        _configuration = configuration;
        client = new ChatClient(
            apiKey: _configuration["OPENAI_API_KEY"],
            model: "o4-mini-2025-04-16"
        );
    }

    public async Task OpenAIServiceFunct(Func<UserModel, JobModel, string> func, UserModel user, JobModel job)
    {
        if (user == null || job == null)
            return;

        ReturnJobModel returnJob = new();
        returnJob.JobName = job.JobName;

        string prompt = func(user, job);

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

            if (char.IsDigit(reply[0]))
            {
                returnJob.JobPercentage = reply[0] + "" + reply[1] + "%, UserSkill match with Job Desc.";
                returnJob.JobKeywords = reply[2..].Trim();
            }
            else
            {
                var (resume, coverLetter) = ParseReply(reply);
                returnJob.Resume = resume;
                returnJob.CoverLetter = coverLetter;
            }
        }
        catch (Exception ex)
        {
            returnJob.JobPercentage = "Exception: " + ex.Message;
        }
    }

    public static (ResumeModel Resume, CoverLetterModel CoverLetter) ParseReply(string reply)
    {
        var resume = new ResumeModel();
        var coverLetter = new CoverLetterModel();

        // Helper to safely extract a section by name using Regex
        string ExtractSection(string text, string sectionName, string stopAt = null)
        {
            string pattern = $@"(?<=\b{Regex.Escape(sectionName)}:\s*)([\s\S]*?)(?=\b{Regex.Escape(stopAt)}:|$)";
            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        // TITLE
        resume.TitleKeyword = ExtractSection(reply, "TITLE", "SUMMARY");

        // SUMMARY
        resume.Summary = ExtractSection(reply, "SUMMARY", "SKILLS");

        // SKILLS
        var skillsText = ExtractSection(reply, "SKILLS", "PROJECTS");
        if (!string.IsNullOrWhiteSpace(skillsText))
        {
            foreach (var line in skillsText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("Programming Languages:", StringComparison.OrdinalIgnoreCase))
                    resume.ProgramingLanguage = line.Split(':')[1].Trim();
                else if (line.StartsWith("Frameworks & Libraries:", StringComparison.OrdinalIgnoreCase))
                    resume.Frameworks = line.Split(':')[1].Trim();
                else if (line.StartsWith("Relevant Keywords:", StringComparison.OrdinalIgnoreCase))
                    resume.RelevantKeywords = line.Split(':')[1].Trim();
            }
        }

        // PROJECTS
        var projectsText = ExtractSection(reply, "PROJECTS", "WORK EXPERIENCE");
        var projects = new List<ProjectsModel>();
        var projectMatches = Regex.Split(projectsText, @"-\s\*\*")
                                  .Where(p => !string.IsNullOrWhiteSpace(p));

        foreach (var chunk in projectMatches)
        {
            var lines = chunk.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            if (lines.Count < 2) continue;

            var project = new ProjectsModel
            {
                ProjectName = lines[0].Replace("**", "").Trim(),
                Description = lines.ElementAtOrDefault(1) ?? ""
            };

            var bullets = lines.Where(l => l.StartsWith("•") || l.StartsWith("-")).ToList();
            if (bullets.Count > 0) project.Bullet1 = bullets[0].TrimStart('•', '-', ' ').Trim();
            if (bullets.Count > 1) project.Bullet2 = bullets[1].TrimStart('•', '-', ' ').Trim();
            if (bullets.Count > 2) project.Bullet3 = bullets[2].TrimStart('•', '-', ' ').Trim();

            projects.Add(project);
        }
        resume.Projects = projects;

        // COVER LETTER
        var coverLetterText = ExtractSection(reply, "COVER LETTER");
        if (!string.IsNullOrWhiteSpace(coverLetterText))
        {
            var titleMatch = Regex.Match(coverLetterText, @"Title:\s*(.*)", RegexOptions.IgnoreCase);
            if (titleMatch.Success)
                coverLetter.Title = titleMatch.Groups[1].Value.Trim();

            coverLetter.Body1 = ExtractSection(coverLetterText, "Body 1", "Body 2");
            coverLetter.Body2 = ExtractSection(coverLetterText, "Body 2", "Body 3");
            coverLetter.Body3 = ExtractSection(coverLetterText, "Body 3");
        }

        return (resume, coverLetter);
    }
}
