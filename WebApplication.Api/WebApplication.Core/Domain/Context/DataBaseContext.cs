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
            .OnDelete(DeleteBehavior.Cascade);

            // Ogłoszenia -> Użytkownik
            modelBuilder.Entity<Advertisement>()  // Relacja n..1
            .HasOne(a => a.Relation)              // Relacja Advs.UserId do klasy Users
            .WithMany(b => b.Advertisements)      // Wysłanie ogłoszeń do tablicy advs w klasie Users
            .OnDelete(DeleteBehavior.Restrict);

            // Miasta -> Ogłoszenia
            modelBuilder.Entity<Advertisement>()  // Relacja  1..1
            .HasOne(a => a.CityRel)               // Relacja Advs.City do klasy Cities
            .WithOne(b => b.Advertisement);       // Przypisanie danych z relacji do wirutalnego pola w Cities.Advs typu Advs

            modelBuilder.Entity<Message>()
            .HasOne(a => a.Relation)
            .WithMany(b => b.Messages);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Voivodeship> Voivodeships { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
