namespace resume_builder_api.Models;

public class ReturnJobModel
{
    public string? JobName { get; set; } = string.Empty;
    public ResumeModel? Resume { get; set; } = new ResumeModel();
    public CoverLetterModel? CoverLetter { get; set; } = new CoverLetterModel();
    public string? JobKeywords { get; set; } = string.Empty;
    public string? JobPercentage { get; set; } = string.Empty;
}
