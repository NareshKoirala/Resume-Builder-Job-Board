namespace Desktop.Resume_Builder_API.resume_builder_api.Models;

public class EducationEntry
{
    public int Id { get; set; }
    public int UserModelId { get; set; }
    public string? InstitutionName { get; set; }
    public string? Location { get; set; }
    public string? Date { get; set; }
    public string? Details { get; set; }
}
