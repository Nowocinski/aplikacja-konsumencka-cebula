using Microsoft.EntityFrameworkCore;

namespace WebApplication.Core.Domain.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(column =>
            {
                column.Property(name => name.FirstName)
                      .HasColumnType("nvarchar(20)")
                      .IsRequired(true);
                column.Property(name => name.LastName)
                      .HasColumnType("nvarchar(30)")
                      .IsRequired(true);
                column.Property(name => name.PhoneNumber)
                      .HasColumnType("nvarchar(11)")
                      .IsRequired(true);
                column.Property(name => name.Email)
                      .HasColumnType("nvarchar(40)")
                      .IsRequired(true);
                column.Property(name => name.Password)
                      .HasColumnType("nvarchar(100)")
                      .IsRequired(true);

            });

            modelBuilder.Entity<Message>(column =>
            {
                column.Property(name => name.Sender_Id)
                      .HasColumnType("uniqueidentifier")
                      .IsRequired(true);
                column.Property(name => name.Recipient_Id)
                      .HasColumnType("uniqueidentifier")
                      .IsRequired(true);
                column.Property(name => name.Contents)
                      .HasColumnType("nvarchar(MAX)")
                      .IsRequired(true);
                column.Property(name => name.Date)
                      .HasColumnType("datetime")
                      .IsRequired(true);
            });

            modelBuilder.Entity<Advertisement>(column =>
            {
                column.Property(name => name.User_Id)
                      .HasColumnType("uniqueidentifier")
                      .IsRequired(true);
                column.Property(name => name.Title)
                      .HasColumnType("nvarchar(25)")
                      .IsRequired(true);
                column.Property(name => name.Description)
                      .HasColumnType("nvarchar(500)")
                      .IsRequired(true);
                column.Property(name => name.Price)
                      .HasColumnType("decimal(20, 2)")
                      .IsRequired(true);
                column.Property(name => name.City_id)
                      .HasColumnType("int")
                      .IsRequired(true);
                column.Property(name => name.Street)
                      .HasColumnType("nvarchar(50)")
                      .IsRequired(true);
                column.Property(name => name.Floor)
                      .HasColumnType("int")
                      .IsRequired(false);
                column.Property(name => name.Size)
                      .HasColumnType("decimal(20,2)")
                      .IsRequired(true);
                column.Property(name => name.Category)
                      .HasColumnType("nvarchar(30)")
                      .IsRequired(true);
                column.Property(name => name.Date)
                      .HasColumnType("datetime")
                      .IsRequired(true);
            });

            modelBuilder.Entity<AdvertisementImage>(column =>
            {
                column.Property(name => name.Advertisement_Id)
                      .HasColumnType("uniqueidentifier")
                      .IsRequired(true);
                column.Property(name => name.Image)
                      .HasColumnType("nvarchar(MAX)")
                      .IsRequired(true);
                column.Property(name => name.Description)
                      .HasColumnType("nvarchar(100)")
                      .IsRequired(true);
                column.Property(name => name.Name)
                      .HasColumnType("nvarchar(100)")
                      .IsRequired(true);
            });

            modelBuilder.Entity<Voivodeship>(column =>
            {
                column.Property(name => name.Name)
                      .HasColumnType("nvarchar(30)")
                      .IsRequired(true);
            });

            modelBuilder.Entity<City>(column =>
            {
                column.Property(name => name.Name)
                      .HasColumnType("nvarchar(40)")
                      .IsRequired(true);
                column.Property(name => name.Voivodeship_Id)
                      .HasColumnType("int")
                      .IsRequired(true);
            });

            modelBuilder.Entity<Advertisement>()
                        .HasMany(a => a.Images)
                        .WithOne()
                        .HasForeignKey(x => x.Advertisement_Id)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Advertisement>()
                        .HasOne(a => a.User)
                        .WithMany(b => b.Advertisements)
                        .HasForeignKey(x => x.User_Id)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Advertisement>()
                        .HasOne(a => a.City)
                        .WithMany(b => b.Advertisement)
                        .HasForeignKey(x => x.City_id)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                        .HasOne(a => a.User)
                        .WithMany(b => b.Messages)
                        .HasForeignKey(x => x.Sender_Id)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<City>()
                        .HasOne(g => g.Voivodeship)
                        .WithMany()
                        .HasForeignKey(s => s.Voivodeship_Id);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Voivodeship> Voivodeships { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
