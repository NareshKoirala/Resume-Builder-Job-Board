using SQLite;
using Resume_Builder_MAUI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resume_Builder_MAUI.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
        }

        public async Task EmptyAllTablesAsync() 
        {
            await _db.DeleteAllAsync<UserModel>();
            await _db.DeleteAllAsync<EducationEntry>();
            await _db.DeleteAllAsync<WorkEntry>();
            await _db.DeleteAllAsync<CertificateEntry>();
            await _db.DeleteAllAsync<SkillEntry>();
            await _db.DeleteAllAsync<ProjectEntry>();
        }

        public async Task InitializeTablesAsync()
        {
            await _db.CreateTableAsync<UserModel>();
            await _db.CreateTableAsync<EducationEntry>();
            await _db.CreateTableAsync<WorkEntry>();
            await _db.CreateTableAsync<CertificateEntry>();
            await _db.CreateTableAsync<SkillEntry>();
            await _db.CreateTableAsync<ProjectEntry>();
        }

        public async Task<int> SaveUserAsync(UserModel user)
        {
            if (user.Id == 0)
                return await _db.InsertAsync(user);
            else
                return await _db.UpdateAsync(user);
        }

        public async Task<int> SaveEducationAsync(EducationEntry entry)
        {
            var exists = await _db.Table<EducationEntry>()
                .Where(e => e.InstitutionName == entry.InstitutionName &&
                            e.Location == entry.Location &&
                            e.Date == entry.Date)
                .FirstOrDefaultAsync();

            if (exists != null)
                return 0;

            return await _db.InsertAsync(entry);
        }

        public async Task<int> SaveWorkAsync(WorkEntry entry)
        {
            var exists = await _db.Table<WorkEntry>()
                .Where(e => e.CompanyName == entry.CompanyName &&
                            e.Location == entry.Location &&
                            e.Date == entry.Date)
                .FirstOrDefaultAsync();

            if (exists != null)
                return 0;

            return await _db.InsertAsync(entry);
        }

        public async Task<int> SaveCertificationAsync(CertificateEntry entry)
        {
            var exists = await _db.Table<CertificateEntry>()
                .Where(e => e.CertificateName == entry.CertificateName &&
                            e.Details == entry.Details)
                .FirstOrDefaultAsync();

            if (exists != null)
                return 0;

            return await _db.InsertAsync(entry);
        }

        public async Task<int> SaveSkillAsync(SkillEntry entry)
        {
            var exists = await _db.Table<SkillEntry>()
                .Where(e => e.SkillName == entry.SkillName)
                .FirstOrDefaultAsync();

            if (exists != null)
                return 0;

            return await _db.InsertAsync(entry);
        }

        public async Task<int> SaveProjectAsync(ProjectEntry entry)
        {
            var exists = await _db.Table<ProjectEntry>()
                .Where(e => e.ProjectName == entry.ProjectName &&
                            e.Description == entry.Description)
                .FirstOrDefaultAsync();

            if (exists != null)
                return 0;

            return await _db.InsertAsync(entry);
        }

        public async Task<UserModel?> GetUserAsync()
        {
            var user = await _db.Table<UserModel>().FirstOrDefaultAsync();
            if (user != null)
            {
                user.Education = await _db.Table<EducationEntry>().ToListAsync();
                user.WorkExperience = await _db.Table<WorkEntry>().ToListAsync();
                user.Certificates = await _db.Table<CertificateEntry>().ToListAsync();
                user.Skills = await _db.Table<SkillEntry>().ToListAsync();
                user.Projects = await _db.Table<ProjectEntry>().ToListAsync();
            }
            return user;
        }

        public async Task<List<EducationEntry>> GetEducationAsync()
        {
            return await _db.Table<EducationEntry>().ToListAsync();
        }

        public async Task<List<WorkEntry>> GetWorkAsync()
        {
            return await _db.Table<WorkEntry>().ToListAsync();
        }

        public async Task<List<CertificateEntry>> GetCertificationsAsync()
        {
            return await _db.Table<CertificateEntry>().ToListAsync();
        }

        public async Task<List<SkillEntry>> GetSkillsAsync()
        {
            return await _db.Table<SkillEntry>().ToListAsync();
        }

        public async Task<List<ProjectEntry>> GetProjectsAsync()
        {
            return await _db.Table<ProjectEntry>().ToListAsync();
        }
    }
}
