using OpenAI.Chat;
using resume_builder_api.DTOs;
using resume_builder_api.Models;
using System.Data;
using System.Text.RegularExpressions;

namespace resume_builder_api.Services;

public class OpenAIService
{
    private readonly IConfiguration _configuration;

    private readonly ChatClient client;

    public OpenAIService(IConfiguration configuration)
    {
        _configuration = configuration;
        client = new ChatClient(
            apiKey: _configuration["OPENAI_API_KEY"],
            model: "o4-mini-2025-08-07"
        );
    }

    public async Task<ReturnJobModel> OpenAIServiceFunct(Func<UserModel, JobDto, string> func, UserModel user, JobDto job, ReturnJobModel returnJob)
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

        // Helper: extract any section by title
        string GetSectionContent(string sectionTitle)
        {
            var match = Regex.Match(reply,
                $@"{sectionTitle}:\s*([\s\S]*?)(?=\n[A-Za-z ]+:|$)",
                RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        // Basic fields
        resume.TitleKeyword = GetSectionContent("TITLE");
        resume.Summary = GetSectionContent("SUMMARY");

        // Skills
        var skillsText = GetSectionContent("SKILLS");
        if (!string.IsNullOrWhiteSpace(skillsText))
        {
            string ExtractGroup(Match m) => m.Success ? m.Groups[1].Value.Trim() : "";

            resume.ProgramingLanguage = ExtractGroup(Regex.Match(skillsText, @"Programming Languages\s*:?\s*(.+?)(?=Frameworks|Relevant|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline));
            resume.Frameworks = ExtractGroup(Regex.Match(skillsText, @"Frameworks\s*&?\s*Libraries\s*:?\s*(.+?)(?=Relevant|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline));
            resume.RelevantKeywords = ExtractGroup(Regex.Match(skillsText, @"Relevant Keywords\s*:?\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Singleline));
        }

        // Fallbacks
        if (string.IsNullOrWhiteSpace(resume.ProgramingLanguage))
            resume.ProgramingLanguage = GetSectionContent("PROGRAMMING LANGUAGES");
        if (string.IsNullOrWhiteSpace(resume.Frameworks))
            resume.Frameworks = GetSectionContent("FRAMEWORKS & LIBRARIES");
        if (string.IsNullOrWhiteSpace(resume.RelevantKeywords))
            resume.RelevantKeywords = GetSectionContent("RELEVANT KEYWORDS");

        // Projects
        var projectsText = GetSectionContent("PROJECTS");
        var projects = new List<ProjectsModel>();
        if (!string.IsNullOrWhiteSpace(projectsText))
        {
            // Flexible pattern: matches "- **Project Title**" OR "- Project Title -"
            var projectPattern = @"-\s*(\*\*(?<name1>.+?)\*\*|(?<name2>.+?)-)\s*(?<desc>.*?)(?=\n-\s*(\*\*|[A-Z])|$)";
            foreach (Match match in Regex.Matches(projectsText, projectPattern, RegexOptions.Singleline))
            {
                var name = !string.IsNullOrWhiteSpace(match.Groups["name1"].Value)
                            ? match.Groups["name1"].Value.Trim()
                            : match.Groups["name2"].Value.Trim('-').Trim();

                var details = match.Groups["desc"].Value.Trim();

                var lines = details.Split('\n')
                                   .Select(l => l.Trim())
                                   .Where(l => !string.IsNullOrWhiteSpace(l))
                                   .ToList();

                var project = new ProjectsModel
                {
                    ProjectName = name,
                    Description = lines.FirstOrDefault() ?? ""
                };

                // Capture bullets
                var bullets = lines.Where(l => Regex.IsMatch(l, @"^(\•|-)\s?")).ToList();
                if (bullets.Count > 0) project.Bullet1 = bullets[0].TrimStart('•', '-', ' ').Trim();
                if (bullets.Count > 1) project.Bullet2 = bullets[1].TrimStart('•', '-', ' ').Trim();
                if (bullets.Count > 2) project.Bullet3 = bullets[2].TrimStart('•', '-', ' ').Trim();

                projects.Add(project);
            }
        }
        resume.Projects = projects;

        // Work Experience
        var workExpText = GetSectionContent("WORK EXPERIENCE");
        if (!string.IsNullOrWhiteSpace(workExpText))
            resume.WorkExperience.Add(workExpText);

        // Education
        var educationText = GetSectionContent("EDUCATION");
        if (!string.IsNullOrWhiteSpace(educationText))
        {
            var eduSplit = educationText.Trim().Split('\n');
            var educationModel = new EducationModel
            {
                Title = eduSplit.FirstOrDefault()?.Trim() ?? "",
                Details = string.Join("\n", eduSplit.Skip(1).Select(e => e.Trim()))
            };
            resume.Education.Add(educationModel);
        }

        // Certificates
        var certificationText = GetSectionContent("CERTIFICATES");
        if (!string.IsNullOrWhiteSpace(certificationText))
        {
            var certSplit = certificationText.Split('\n');
            foreach (var i in certSplit.Where(c => !string.IsNullOrWhiteSpace(c)))
                resume.Certificates.Add(new CertificationModel { Details = i.Trim() });
        }

        // Cover Letter
        coverLetter.Title = GetSectionContent("COVER LETTER");
        coverLetter.Body1 = GetSectionContent("Body 1");
        coverLetter.Body2 = GetSectionContent("Body 2");
        coverLetter.Body3 = GetSectionContent("Body 3");

        return (resume, coverLetter);
    }

}
