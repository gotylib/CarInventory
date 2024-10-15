using CarInventory.CarInventory.Dal.BaseObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.CarInventory.Dal
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Создание таблицы Cars (Пример)
            builder.Entity<Car>(entity =>
            {
                entity.ToTable("Cars");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Make).IsRequired();
                entity.Property(e => e.Model).IsRequired();
                entity.Property(e => e.Color).IsRequired();
                entity.Property(e => e.IsAvailable).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
            });

        }
    }

}
