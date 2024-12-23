using Microsoft.EntityFrameworkCore;	
using Microsoft.Identity.Client;


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
		}

}
