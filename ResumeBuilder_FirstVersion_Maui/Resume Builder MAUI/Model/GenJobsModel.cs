
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume_Builder_MAUI.Model
{
    public class GenJobsModel
    {
        public string? JobId { get; set; } = string.Empty;
        public string? JobName { get; set; } = string.Empty;
        public string? ApplyLink { get; set; } = string.Empty;
        public JobsModel? Jobs { get; set; } = new JobsModel();
        public ResumeModel? Resume { get; set; } = new ResumeModel();
        public CoverLetterModel? CoverLetter { get; set; } = new CoverLetterModel();
        public string? jobKeywords { get; set; } = string.Empty;
        public string? jobPercentage { get; set; } = string.Empty;
    }
}
