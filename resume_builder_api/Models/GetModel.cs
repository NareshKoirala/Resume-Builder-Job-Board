namespace Desktop.Resume_Builder_API.resume_builder_api.Models;

public class JobModel
{
    public string JobName { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;
}

public class UserModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string JobField { get; set; } = string.Empty;
    public string PortfolioUrl { get; set; } = string.Empty;
    public string LinkedInUrl { get; set; } = string.Empty;
    public string UserSummary { get; set; } = string.Empty;
    public List<EducationEntry> Education { get; set; } = [];
    public List<WorkEntry> WorkExperience { get; set; } = [];
    public List<CertificateEntry> Certificates { get; set; } = [];
    public List<SkillEntry> Skills { get; set; } = [];
    public List<ProjectEntry> Projects { get; set; } = [];
}

public class EducationEntry
{
    public string InstitutionName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}

public class SkillEntry
{
    public int Id { get; set; }
    public string SkillName { get; set; } = string.Empty;
}

public class ProjectEntry
{
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class WorkEntry
{
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}

public class CertificateEntry
{
    public string CertificateName { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}