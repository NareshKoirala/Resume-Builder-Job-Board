using Microsoft.EntityFrameworkCore;
using Desktop.Resume_Builder_API.resume_builder_api.Models;

namespace Desktop.Resume_Builder_API.resume_builder_api.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<EducationEntry> EducationEntries { get; set; }
    public DbSet<WorkEntry> WorkEntries { get; set; }
    public DbSet<CertificateEntry> CertificateEntries { get; set; }
    public DbSet<SkillEntry> SkillEntries { get; set; }
    public DbSet<ProjectEntry> ProjectEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>()
            .Property(u => u.PublicId)
            .HasDefaultValueSql("encode(gen_random_bytes(8), 'hex')");

        modelBuilder.Entity<UserModel>()
            .HasIndex(u => u.PublicId)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
