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

        // DbSet för alla entiteter
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


        //Ska vara namnet på modellkalsserna
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definiera tabellnamn för användare
            modelBuilder.Entity<User>().ToTable("Users");

            //1-1 förhållande CV
            modelBuilder.Entity<CV>()
                .HasOne(c => c.User)                // Ett CV har en User
                .WithOne(u => u.CV)                 // En User har ett CV
                .HasForeignKey<CV>(c => c.UserId)   // Använd UserId som foreign key i CV
                .OnDelete(DeleteBehavior.Cascade);  // Om du vill att CV ska tas bort om användaren tas bort

            // Konfigurera relationen mellan Project och User
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany()  // Om en User kan ha flera projekt (1:N relation)
                .HasForeignKey(p => p.CreatedBy)
                .HasPrincipalKey(u => u.Id) // Antar att User har en "Id"-egenskap
                .OnDelete(DeleteBehavior.Restrict);  // Ange önskat beteende vid borttagning av användare

            // Många-till-många relation mellan User och Project via UserInProject
            modelBuilder.Entity<UserInProject>()
                .HasOne(uip => uip.User)
                .WithMany(u => u.UserInProjects)  // Ändrat från UserInProject till UserInProjects
                .HasForeignKey(uip => uip.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserInProject>()
                .HasOne(uip => uip.Project)
                .WithMany(p => p.UsersInProject)  // Ändrat från UserInProject till UserInProjects
                .HasForeignKey(uip => uip.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // Många-till-många relation mellan CV och Kompetenser via CvCompetences
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

            // Många-till-många relation mellan CV och Utbildning via CvEducation
            modelBuilder.Entity<CvEducation>()
                .HasKey(ce => new { ce.CVID, ce.EducationID });

            modelBuilder.Entity<CvEducation>()
                .HasOne(ce => ce.CV)
                .WithMany(c => c.Educations)
                .HasForeignKey(ce => ce.CVID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CvEducation>()
                .HasOne(cv => cv.Education)//CV
                .WithMany(e => e.CVEducations)
                .HasForeignKey(ce => ce.EducationID)
                .OnDelete(DeleteBehavior.NoAction);

            // Många-till-många relation mellan CV och ArbetsErfarenhet via CvWorkExperience
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

            // Definiera relationen för Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.FranUser)
                .WithMany() // En användare kan ha många meddelanden som avsändare
                .HasForeignKey(m => m.FranUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.TillUser)
                .WithMany() // En användare kan ha många meddelanden som mottagare
                .HasForeignKey(m => m.TillUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
