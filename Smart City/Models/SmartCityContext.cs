using Microsoft.EntityFrameworkCore;

namespace Smart_City.Models;

public class SmartCityContext : DbContext
{
	public SmartCityContext(DbContextOptions<SmartCityContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
	public DbSet<Complaint> Complaints { get; set; }
	public DbSet<Suggestion> Suggestions { get; set; }
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<Bill> Bills { get; set; }
	public DbSet<UtilityIssue> UtilityIssues { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
		   .HasDiscriminator<string>("Discriminator")
		   .HasValue<Citizen>("Citizen")
		   .HasValue<Admin>("Admin");
	}


	// connection string
	//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//{
	//    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=SmartCity;Integrated Security=True;TrustServerCertificate=True;");

	//}

}
