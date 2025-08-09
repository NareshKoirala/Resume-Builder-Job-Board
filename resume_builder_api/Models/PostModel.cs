namespace Desktop.Resume_Builder_API.resume_builder_api.Models;

public class ReturnJobModel
{
    public string? JobName { get; set; } = string.Empty;
    public ResumeModel? Resume { get; set; } = new ResumeModel();
    public CoverLetterModel? CoverLetter { get; set; } = new CoverLetterModel();
    public string? JobKeywords { get; set; } = string.Empty;
    public string? JobPercentage { get; set; } = string.Empty;
}

public class CoverLetterModel
{
    public string Title { get; set; } = string.Empty;
    public string Body1 { get; set; } = string.Empty;
    public string Body2 { get; set; } = string.Empty;
    public string Body3 { get; set; } = string.Empty;
}

public class ResumeModel
{
    public string TitleKeyword { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ProgramingLanguage { get; set; } = string.Empty;
    public string Frameworks { get; set; } = string.Empty;
    public string RelevantKeywords { get; set; } = string.Empty;
    public List<ProjectsModel> Projects { get; set; } = new List<ProjectsModel>();
    public string WorkExperience { get; set; } = string.Empty;
    public string Education { get; set; } = string.Empty;
    public string Certificates { get; set; } = string.Empty;
}

public class ProjectsModel
{
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Bullet1 { get; set; } = string.Empty;
    public string Bullet2 { get; set; } = string.Empty;
    public string Bullet3 { get; set; } = string.Empty;
}
