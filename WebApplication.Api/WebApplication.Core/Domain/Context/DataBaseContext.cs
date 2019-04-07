using Microsoft.EntityFrameworkCore;

namespace WebApplication.Core.Domain.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Obrazki -> Ogłoszenie
            modelBuilder.Entity<AdvertisementImage>()
            .HasOne(a => a.Relation)
            .WithMany(b => b.Images)
            .OnDelete(DeleteBehavior.Restrict);

            // Ogłoszenia -> Użytkownik
            modelBuilder.Entity<Advertisement>()
            .HasOne(a => a.Relation)
            .WithMany(b => b.Advertisements)
            .OnDelete(DeleteBehavior.Restrict);

            // Ogłoszenia -> Użytkownik
            modelBuilder.Entity<Advertisement>()
            .HasOne(a => a.CityRel)
            .WithOne(b => b.Advertisement);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Voivodeship> Voivodeships { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
