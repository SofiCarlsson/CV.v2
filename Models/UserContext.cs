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
        public DbSet<CV> CVs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Se till att IdentityUserLogin har en primärnyckel
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => e.UserId); // Sätt primärnyckel
            });

            // Konfigurera relationen mellan User och CV
            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.CV)  // En användare har ett CV
            //    .WithOne(c => c.User)  // Ett CV tillhör en användare
            //    .HasForeignKey<CV>(c => c.UserId)  // Koppla UserId i CV till användarens Id
            //    .OnDelete(DeleteBehavior.Cascade);  // Ta bort CV om användaren tas bort

            // Konfigurera relationen mellan Project och User
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)  // Ett projekt har en skapare (User)
                .WithMany(u => u.CreatedProjects)  // En användare kan ha flera skapade projekt
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);  // Ta bort projekt om användaren tas bort

            // Konfigurera relationen mellan UserInProject, User och Project
            modelBuilder.Entity<UserInProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });  // Sätt sammansatt primärnyckel

            modelBuilder.Entity<UserInProject>()
                .HasOne(up => up.User)  // En användare kan vara i många projekt
                .WithMany(u => u.Projects)  // En användare kan vara kopplad till många projekt
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Ta bort UserInProject om användaren tas bort

            modelBuilder.Entity<UserInProject>()
                .HasOne(up => up.Project)  // Ett projekt kan ha många användare
                .WithMany(p => p.UsersInProject)  // Ett projekt kan vara kopplat till många användare
                .HasForeignKey(up => up.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);  // Ta bort UserInProject om projektet tas bort
        }
    }
}

