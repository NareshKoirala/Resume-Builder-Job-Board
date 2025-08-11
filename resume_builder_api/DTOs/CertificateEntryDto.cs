using System.ComponentModel.DataAnnotations;

namespace ResumeBuilderApi.DTOs
{
    public class CreateCertificateEntryDto
    {
        [Required]
        [StringLength(200)]
        public string? CertificateName { get; set; }

        [Required]
        [StringLength(2000)]
        public string? Details { get; set; }
    }
}
