namespace Desktop.Resume_Builder_API.resume_builder_api.Models;

public class CertificateEntry
{
    public int Id { get; set; }
    public int UserModelId { get; set; }
    public string? CertificateName { get; set; }
    public string? Details { get; set; }
}
