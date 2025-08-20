using resume_builder_api.Models;
using Microsoft.EntityFrameworkCore;
using resume_builder_api.DTOs;

namespace resume_builder_api.Services
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
    }
}