using Clinical_Management_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinical_Management_System.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
