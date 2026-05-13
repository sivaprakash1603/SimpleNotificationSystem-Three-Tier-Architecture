using NotificationModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace NotificationDALLibrary.Contexts
{
    public class NotificationContext : DbContext
    {
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=NotificationSystem;Username=postgres;Password=postgres");
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure User entity
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(u => u.Email).HasName("PK_User");
                u.Property(u => u.Name).IsRequired().HasMaxLength(100);
                u.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(20);
                u.HasData(
                    new User { Name = "siva", Email = "siva@gmail.com", PhoneNumber = "9787899545" }
                );
            });

            // Configure Notification entity
            modelBuilder.Entity<Notification>(n =>
            {
                n.HasKey(n => n.Id).HasName("PK_Notification");
                n.Property(n => n.Message).IsRequired();
                n.Property(n => n.SentDate)
                    .IsRequired()
                    .HasConversion(
                        value => value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime(),
                        value => DateTime.SpecifyKind(value, DateTimeKind.Utc));
                n.Property(n => n.UserEmail).IsRequired();

                // Define relationship with User
                n.HasOne(a => a.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserEmail)
                        .OnDelete(DeleteBehavior.Cascade);

                n.HasDiscriminator<string>("NotificationType")
                    .HasValue<EmailNotification>("email")
                    .HasValue<SmsNotification>("sms");
            });

            modelBuilder.Entity<EmailNotification>().HasData(
                new EmailNotification
                {
                    Id = 1,
                    Message = "Welcome to our service!",
                    SentDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UserEmail = "siva@gmail.com",
                    ToEmail = "siva@gmail.com"
                }
            );
        }
    }
}