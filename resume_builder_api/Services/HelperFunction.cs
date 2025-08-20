using Desktop.Resume_Builder_API.resume_builder_api.DTOs;
using Desktop.Resume_Builder_API.resume_builder_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Desktop.Resume_Builder_API.resume_builder_api.Services
{
    public static class HelperFunction
    {
        public static async Task<UserModel> FetchUserModel(JobDTO jobDto, AppDbContext _dbContext)
        {
            // Implementation for fetching UserModel from JobDTO
            // Get user from database using publicID
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.PublicId == jobDto.PublicId);

            if (user == null)
            {
                return new UserModel { JobField = "User with PublicId not found. Highly likely DB issue." };
            }

            user.Certificates = await _dbContext.CertificateEntries.Where(c => c.UserModelId == user.Id).ToListAsync();
            user.Education = await _dbContext.EducationEntries.Where(e => e.UserModelId == user.Id).ToListAsync();
            user.Projects = await _dbContext.ProjectEntries.Where(p => p.UserModelId == user.Id).ToListAsync();
            user.Skills = await _dbContext.SkillEntries.Where(s => s.UserModelId == user.Id).ToListAsync();
            user.WorkExperience = await _dbContext.WorkEntries.Where(w => w.UserModelId == user.Id).ToListAsync();

            return user;
        }
        public static UserModel ReturnUserModel(UserRegisterDto user)
        {
            return new UserModel
            {
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