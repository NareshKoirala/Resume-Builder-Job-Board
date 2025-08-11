using Desktop.Resume_Builder_API.resume_builder_api.DTOs;
using Desktop.Resume_Builder_API.resume_builder_api.Models;

namespace Desktop.Resume_Builder_API.resume_builder_api.Services
{
    public static class HelperFunction
    {
        public static UserModel ReturnUserModel(UserRegisterDto user)
        {
            return new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Mobile = user.Mobile,
                Location = user.Location,
                Province = user.Province,
                JobField = user.JobField,
                PortfolioUrl = user.PortfolioUrl,
                LinkedInUrl = user.LinkedInUrl,
                UserSummary = user.UserSummary,
                Education = [.. user.Education.Select(e => new EducationEntry
                {
                    InstitutionName = e.InstitutionName,
                    Date = e.Date,
                    Location = e.Location,
                    Details = e.Details
                })],
                WorkExperience = [.. user.WorkExperience.Select(w => new WorkEntry
                {
                    CompanyName = w.CompanyName,
                    Location = w.Location,
                    Details = w.Details
                })],
                Certificates = [.. user.Certificates.Select(c => new CertificateEntry
                {
                    CertificateName = c.CertificateName,
                    Details = c.Details
                })],
                Skills = [.. user.Skills.Select(s => new SkillEntry
                {
                    SkillName = s.SkillName
                })],
                Projects = [.. user.Projects.Select(p => new ProjectEntry
                {
                    ProjectName = p.ProjectName,
                    Description = p.Description
                })]
            };
        }
    }
}