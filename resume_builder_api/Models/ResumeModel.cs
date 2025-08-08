namespace Desktop.Resume_Builder_API.resume_builder_api.Models;

public class ResumeModel
{
    public string TitleKeyword { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ProgramingLanguage { get; set; } = string.Empty;
    public string Frameworks { get; set; } = string.Empty;
    public string RelevantKeywords { get; set; } = string.Empty;
    public List<ProjectsModel> Projects { get; set; } = new List<ProjectsModel>();
}

public class ProjectsModel
{
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Bullet1 { get; set; } = string.Empty;
    public string Bullet2 { get; set; } = string.Empty;
    public string Bullet3 { get; set; } = string.Empty;
}
