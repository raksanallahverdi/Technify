using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Technify.Entities;
namespace Technify.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {


}
	public DbSet<Work> Works { get; set; }
	public DbSet<ContactMessage> ContactMessages { get; set; }	
	public DbSet<WorkType> WorkTypes { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<WorkType>().HasData(
			new WorkType { Id = 1, Name = "UX/UI Design" },
			new WorkType { Id = 2, Name = "Frontend" },
			new WorkType { Id = 3, Name = "Backend" },
			new WorkType { Id = 4, Name = "HelpDesk" },
			new WorkType { Id = 5, Name = "DevOps" },
			new WorkType { Id = 6, Name = "Cybersecurity" }
		);
	}

}
