using Microsoft.EntityFrameworkCore;

namespace CV_v2.Models
{
	public class UserContext : DbContext
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<CV> CVs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CV>().HasData(
				new CV
				{
					CVId = 1,
					Competences = "SQL",
					Education = "Örebro Universitet",
					WorkExperience = "Tränare",
				},
				new CV
				{
					CVId = 2,
					Competences = "C#",
					Education = "Örebro Universitet",
					WorkExperience = "Arla Foods",
				},
				new CV
				{
					CVId = 3,
					Competences = "CSS, HTML",
					Education = "Örebro Universitet",
					WorkExperience = "PostNord",
				},
				new CV
				{
					CVId = 4,
					Competences = "Java",
					Education = "Örebro Universitet",
					WorkExperience = "IKEA",
				}
			);

			modelBuilder.Entity<User>().HasData(
				new User
				{
					UserId = 1,
					Firstname = "Clara",
					Lastname = "Lunak",
					Email = "clara.lunak04@gmail.com",
					Password = "svampnisse",
					CVID = 1
				},
				new User
				{
					UserId = 2,
					Firstname = "Sofi",
					Lastname = "Carlsson",
					Email = "carlssonsofi99@gmail.com",
					Password = "juan123",
					CVID = 2
				},
				new User
				{
					UserId = 3,
					Firstname = "Olivia",
					Lastname = "Cleve",
					Email = "olivia.cleve@gmail.com",
					Password = "olivia123",
					CVID = 3
				},
				new User
				{
					UserId = 4,
					Firstname = "Malin",
					Lastname = "Sandberg",
					Email = "malin.sandberg@gmail.com",
					Password = "malin123",
					CVID = 4
				}
			);
		}
	}
}
