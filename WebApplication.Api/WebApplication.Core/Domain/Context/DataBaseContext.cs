using Microsoft.EntityFrameworkCore;

namespace WebApplication.Core.Domain.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(column =>
            {
                column.Property(name => name.FirstName)
                      .IsRequired(true)
                      .HasMaxLength(20);
                column.Property(name => name.LastName)
                      .IsRequired(true)
                      .HasMaxLength(30);
                column.Property(name => name.PhoneNumber)
                      .IsRequired(true)
                      .HasMaxLength(11);
                column.Property(name => name.Email)
                      .IsRequired(true)
                      .HasMaxLength(40);
                column.Property(name => name.Password)
                      .IsRequired(true)
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Message>(column =>
            {
                column.Property(name => name.Sender)
                      .IsRequired(true);
                column.Property(name => name.Recipient)
                      .IsRequired(true);
                column.Property(name => name.Contents)
                      .IsRequired(true);
                column.Property(name => name.Date)
                      .IsRequired(true);
            });

            modelBuilder.Entity<Advertisement>(column =>
            {
                column.Property(name => name.UserId)
                      .IsRequired(true);
                column.Property(name => name.Title)
                      .IsRequired(true)
                      .HasMaxLength(25);
                column.Property(name => name.Description)
                      .IsRequired(true)
                      .HasMaxLength(500);
                column.Property(name => name.Price)
                      .IsRequired(true);
                column.Property(name => name.City)
                      .IsRequired(true);
                column.Property(name => name.Street)
                      .IsRequired(true)
                      .HasMaxLength(50);
                column.Property(name => name.Floor)
                      .IsRequired(false);
                column.Property(name => name.Size)
                      .IsRequired(true);
                column.Property(name => name.Category)
                      .IsRequired(true)
                      .HasMaxLength(30);
                column.Property(name => name.Date)
                      .IsRequired(true);
            });

            modelBuilder.Entity<AdvertisementImage>(column =>
            {
                column.Property(name => name.Advertisement)
                      .IsRequired(true);
                column.Property(name => name.Image)
                      .IsRequired(true);
                column.Property(name => name.Description)
                      .IsRequired(true)
                      .HasMaxLength(100);
                column.Property(name => name.Name)
                      .IsRequired(true)
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Voivodeship>(column =>
            {
                column.Property(name => name.Name)
                      .IsRequired(true)
                      .HasMaxLength(30);
            });

            modelBuilder.Entity<City>(column =>
            {
                column.Property(name => name.Name)
                      .IsRequired(true)
                      .HasMaxLength(40);
                column.Property(name => name.Voivodeship)
                      .IsRequired(true);
            });

            modelBuilder.Entity<AdvertisementImage>()
                        .HasOne(a => a.Relation)
                        .WithMany(b => b.Images)
                        .HasForeignKey(x => x.Advertisement)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Advertisement>()
                        .HasOne(a => a.Relation)
                        .WithMany(b => b.Advertisements)
                        .HasForeignKey(x => x.UserId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Advertisement>()
                        .HasOne(a => a.CityRel)
                        .WithOne(b => b.Advertisement)
                        .HasForeignKey<Advertisement>(x => x.City)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                        .HasOne(a => a.Relation)
                        .WithMany(b => b.Messages)
                        .HasForeignKey(x => x.Sender)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<City>()
                        .HasOne(g => g.Relation)
                        .WithMany()
                        .HasForeignKey(s => s.Voivodeship);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Voivodeship> Voivodeships { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
