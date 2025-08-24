using resume_builder_api.Models;
using Microsoft.EntityFrameworkCore;
using resume_builder_api.DTOs;

namespace resume_builder_api.Services
{
    public static class HelperFunction
    {
        public static void FetchUser(UserModel user, AppDbContext db)
        {
            user.Skills = db.SkillEntries.Where(x => x.UserModelId == user.Id).ToList();
            user.Education = db.EducationEntries.Where(x => x.UserModelId == user.Id).ToList();
            user.Certificates = db.CertificateEntries.Where(x => x.UserModelId == user.Id).ToList();
            user.Projects = db.ProjectEntries.Where(x => x.UserModelId == user.Id).ToList();
            user.WorkExperience = db.WorkEntries.Where(x => x.UserModelId == user.Id).ToList();
        }


    }
}