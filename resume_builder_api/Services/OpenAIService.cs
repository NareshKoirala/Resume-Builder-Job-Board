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

    public async Task<ReturnJobModel> OpenAIServiceFunct(Func<UserModel, JobModel, string> func, UserModel user, JobModel job, ReturnJobModel returnJob)
    {
        if (user == null || job == null)
            return new();

        string prompt = func(user, job);

        if (prompt == "0")
            return new();

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
            return returnJob;
        }
        catch (Exception ex)
        {
            returnJob.JobPercentage = "Exception: " + ex.Message;
            return returnJob;
        }
    }
    public static (ResumeModel Resume, CoverLetterModel CoverLetter) ParseReply(string reply)
    {
        var resume = new ResumeModel();
        var coverLetter = new CoverLetterModel();

        var sections = Regex.Split(reply.Trim(), @"\r?\n\r?\n")
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrEmpty(s))
                            .ToList();

        string GetSectionContent(string sectionTitle)
        {
            var section = sections.FirstOrDefault(s => s.StartsWith(sectionTitle + ":", StringComparison.OrdinalIgnoreCase));
            if (section == null) return string.Empty;
            var index = section.IndexOf(':');
            return index >= 0 ? section.Substring(index + 1).Trim() : section.Trim();
        }

        resume.TitleKeyword = GetSectionContent("TITLE");
        resume.Summary = GetSectionContent("SUMMARY");

        var skillsText = GetSectionContent("SKILLS");
        if (!string.IsNullOrWhiteSpace(skillsText))
        {
            string ExtractGroup(Match m) => m.Success ? m.Groups[1].Value.Trim() : "";

            resume.ProgramingLanguage = ExtractGroup(Regex.Match(skillsText, @"Programming Languages\s*:?\s*(.+?)(?=Frameworks|Relevant|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline));
            resume.Frameworks = ExtractGroup(Regex.Match(skillsText, @"Frameworks\s*&\s*Libraries\s*:?\s*(.+?)(?=Relevant|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline));
            resume.RelevantKeywords = ExtractGroup(Regex.Match(skillsText, @"Relevant Keywords\s*:?\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Singleline));
        }

        if (string.IsNullOrWhiteSpace(resume.RelevantKeywords) || string.IsNullOrWhiteSpace(resume.ProgramingLanguage) || string.IsNullOrWhiteSpace(resume.Frameworks))
        {
            resume.RelevantKeywords = GetSectionContent("RELEVANT KEYWORDS");
            resume.ProgramingLanguage = GetSectionContent("PROGRAMMING LANGUAGES");
            resume.Frameworks = GetSectionContent("FRAMEWORKS & LIBRARIES");
        }
        
        if (string.IsNullOrWhiteSpace(resume.RelevantKeywords) || string.IsNullOrWhiteSpace(resume.ProgramingLanguage) || string.IsNullOrWhiteSpace(resume.Frameworks))
        {
            resume.RelevantKeywords = GetSectionContent("SKILLS");
            resume.ProgramingLanguage = GetSectionContent("SKILLS");
            resume.Frameworks = GetSectionContent("SKILLS");
        }

        var projectsText = GetSectionContent("PROJECTS");
        var projects = new List<ProjectsModel>();

        if (!string.IsNullOrWhiteSpace(projectsText))
        {
            // Split on bold project titles wrapped in **
            var projectMatches = Regex.Split(projectsText, @"\*\*(.+?)\*\*")
                                      .Where(p => !string.IsNullOrWhiteSpace(p))
                                      .ToList();

            for (int i = 0; i + 1 < projectMatches.Count; i += 2)
            {
                var name = projectMatches[i].Trim();
                var details = projectMatches[i + 1].Trim();

                var lines = details.Split('\n')
                                   .Select(l => l.Trim())
                                   .Where(l => !string.IsNullOrWhiteSpace(l))
                                   .ToList();

                var project = new ProjectsModel
                {
                    ProjectName = name,
                    Description = lines.FirstOrDefault() ?? ""
                };

                var bullets = lines.Where(l => Regex.IsMatch(l, @"^(\•|-)\s")).ToList();
                if (bullets.Count > 0) project.Bullet1 = bullets[0].TrimStart('•', '-', ' ').Trim();
                if (bullets.Count > 1) project.Bullet2 = bullets[1].TrimStart('•', '-', ' ').Trim();
                if (bullets.Count > 2) project.Bullet3 = bullets[2].TrimStart('•', '-', ' ').Trim();

                projects.Add(project);
            }
        }
        resume.Projects = projects;

        resume.WorkExperience = GetSectionContent("WORK EXPERIENCE");
        resume.Education = GetSectionContent("EDUCATION");
        resume.Certificates = GetSectionContent("CERTIFICATES");

        // Alternative simple extraction if regex fails
        coverLetter.Title = GetSectionContent("COVER LETTER");
        coverLetter.Body1 = GetSectionContent("Body 1");
        coverLetter.Body2 = GetSectionContent("Body 2");
        coverLetter.Body3 = GetSectionContent("Body 3");

        return (resume, coverLetter);
    }

}
