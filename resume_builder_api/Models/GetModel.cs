namespace Desktop.Resume_Builder_API.resume_builder_api.Models;

public class JobModel
{
    public string JobName { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;
}

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

public class EducationEntry
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key
    public string? InstitutionName { get; set; }
    public string? Location { get; set; }
    public string? Date { get; set; }
    public string? Details { get; set; }
}

public class SkillEntry
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key
    public string? SkillName { get; set; }
}

public class ProjectEntry
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
}

public class WorkEntry
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key
    public string? CompanyName { get; set; }
    public string? Location { get; set; }
    public string? Date { get; set; }
    public string? Details { get; set; }
}

public class CertificateEntry
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key
    public string? CertificateName { get; set; }
    public string? Details { get; set; }
}