namespace resume_builder_api.Models;

public class ResumeModel
{
    public string TitleKeyword { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ProgramingLanguage { get; set; } = string.Empty;
    public string Frameworks { get; set; } = string.Empty;
    public string RelevantKeywords { get; set; } = string.Empty;
    public List<ProjectsModel> Projects { get; set; } = new List<ProjectsModel>();
    public List<string> WorkExperience { get; set; } = new();
    public List<EducationModel> Education { get; set; } = new();
    public List<CertificationModel> Certificates { get; set; } = new();
}
