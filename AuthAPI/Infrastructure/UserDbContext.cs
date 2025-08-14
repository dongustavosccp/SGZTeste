using AuthAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NmUser).IsRequired();
                entity.Property(e => e.DsPassword).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}