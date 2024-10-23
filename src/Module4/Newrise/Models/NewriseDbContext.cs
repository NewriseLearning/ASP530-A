using Microsoft.EntityFrameworkCore;

namespace Newrise.Models {
	public class NewriseDbContext : DbContext {
		public DbSet<Event> Events { get; set; }
		public DbSet<Participant> Participants { get; set; }
		public NewriseDbContext(DbContextOptions<NewriseDbContext> options) : base(options) {

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Event>().Property("Fee").HasColumnType("decimal").HasPrecision(7, 2);
			modelBuilder.Entity<Participant>().HasAlternateKey(p => p.Email);
		}
	}
}
