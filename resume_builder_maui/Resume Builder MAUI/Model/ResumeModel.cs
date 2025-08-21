using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume_Builder_MAUI.Model
{
    public class ResumeModel
    {
        public string TitleKeyword { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string ProgramingLanguage { get; set; } = string.Empty;
        public string Frameworks { get; set; } = string.Empty;
        public string RelevantKeywords { get; set; } = string.Empty;
        public List<ProjectsModel> Projects { get; set; } = new List<ProjectsModel>();
    }

    public class ProjectsModel
    {
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Bullet1 { get; set; } = string.Empty;
        public string Bullet2 { get; set; } = string.Empty;
        public string Bullet3 { get; set; } = string.Empty;
    }
}
