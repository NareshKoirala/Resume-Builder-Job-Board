using SQLite;

namespace Resume_Builder_MAUI.Model;

public class UserModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Basic Info
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Province { get; set; }  = string.Empty;
    public string JobField { get; set; } = string.Empty;
    public string PortfolioUrl { get; set; } = string.Empty;
    public string LinkedInUrl { get; set; } = string.Empty;
    public string UserSummary { get; set; } = string.Empty;

    // Education, Work Experience, Certificates (multiple entries)
    [Ignore]
    public List<EducationEntry> Education { get; set; } = [];
    [Ignore]
    public List<WorkEntry> WorkExperience { get; set; } = [];
    [Ignore] 
    public List<CertificateEntry> Certificates { get; set; } = [];
    [Ignore]
    public List<SkillEntry> Skills { get; set; } = [];
    [Ignore]
    public List<ProjectEntry> Projects { get; set; } = [];
}

public class EducationEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string InstitutionName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}

public class SkillEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string SkillName { get; set; } = string.Empty;
}

public class  ProjectEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class WorkEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}

public class CertificateEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string CertificateName { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}