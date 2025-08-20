namespace resume_builder_api.Models;

public class UserModel
{
    public int Id { get; set; }
    public string? PublicId { get; set; } // DB-generated

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
