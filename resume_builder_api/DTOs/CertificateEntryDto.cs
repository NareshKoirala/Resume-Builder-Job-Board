using System.ComponentModel.DataAnnotations;

namespace resume_builder_api.DTOs
{
    public class CertificateEntryDto
    {
        [Required]
        [StringLength(200)]
        public string? CertificateName { get; set; }

        [Required]
        [StringLength(2000)]
        public string? Details { get; set; }
    }
}
