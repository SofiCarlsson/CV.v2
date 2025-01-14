using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CV_v2.Models
{
    public class UserContext : IdentityDbContext<User>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserInProject> UserInProjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<CvCompetences> CvCompetences { get; set; }
        public DbSet<Competences> Competences { get; set; }
        public DbSet<CvEducation> CvEducations { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<CvWorkExperience> CvWorkExperiences { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<CV>()
                .HasOne(c => c.User)                
                .WithOne(u => u.CV)                 
                .HasForeignKey<CV>(c => c.UserId)   
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany()  
                .HasForeignKey(p => p.CreatedBy)
                .HasPrincipalKey(u => u.Id) 
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<UserInProject>()
                .HasOne(uip => uip.User)
                .WithMany(u => u.UserInProjects)  
                .HasForeignKey(uip => uip.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserInProject>()
                .HasOne(uip => uip.Project)
                .WithMany(p => p.UsersInProject)  
                .HasForeignKey(uip => uip.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CvCompetences>()
                .HasKey(cc => new { cc.CVID, cc.CompetencesID });

            modelBuilder.Entity<CvCompetences>()
                .HasOne(cc => cc.CV)
                .WithMany(c => c.Competences)
                .HasForeignKey(cc => cc.CVID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CvCompetences>()
                .HasOne(cc => cc.Competences)
                .WithMany(c => c.CvCompetences)
                .HasForeignKey(cc => cc.CompetencesID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CvEducation>()
                .HasKey(ce => new { ce.CVID, ce.EducationID });

            modelBuilder.Entity<CvEducation>()
                .HasOne(ce => ce.CV)
                .WithMany(c => c.Educations)
                .HasForeignKey(ce => ce.CVID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CvEducation>()
                .HasOne(cv => cv.Education)
                .WithMany(e => e.CVEducations)
                .HasForeignKey(ce => ce.EducationID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CvWorkExperience>()
                .HasKey(cwe => new { cwe.CVID, cwe.WorkExperienceID });

            modelBuilder.Entity<CvWorkExperience>()
                .HasOne(cwe => cwe.CV)
                .WithMany(c => c.WorkExperiences)
                .HasForeignKey(cwe => cwe.CVID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CvWorkExperience>()
                .HasOne(cwe => cwe.WorkExperience)
                .WithMany(w => w.CVWorkExperiences)
                .HasForeignKey(cwe => cwe.WorkExperienceID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.FranUser)
                .WithMany() 
                .HasForeignKey(m => m.FranUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.TillUser)
                .WithMany() 
                .HasForeignKey(m => m.TillUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
