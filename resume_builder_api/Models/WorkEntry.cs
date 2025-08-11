namespace Desktop.Resume_Builder_API.resume_builder_api.Models;

public class WorkEntry
{
    public int Id { get; set; }
    public int UserModelId { get; set; }
    public string? CompanyName { get; set; }
    public string? Location { get; set; }
    public string? Date { get; set; }
    public string? Details { get; set; }
}
