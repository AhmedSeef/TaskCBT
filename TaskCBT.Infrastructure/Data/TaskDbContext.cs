using Microsoft.EntityFrameworkCore;
using TaskCBT.Domain.Entities;

namespace TaskCBT.Infrastructure.Data
{
    public class TaskDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(c => c.ICNumber).IsUnique();

                entity.Property(c => c.CustomerName).IsRequired();
                entity.Property(c => c.Email).IsRequired();
                entity.Property(c => c.MobileNumber).IsRequired();
            });
        }
    }
}
