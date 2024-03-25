
using Microsoft.EntityFrameworkCore;
using WebApiLibr.Models.EntitiesModel;

namespace WebApiLibr
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        /*
         dotnet ef migrations add InitialCreate --context AppDbContext
         dotnet ef database update
        */
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            .LogTo(Console.WriteLine)
            .UseNpgsql("Host=localhost;Username=postgres;Password=2402;Database=FinalExaminationDb");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Email).IsUnique();

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasOne(x => x.Role)
                    .WithMany(x => x.Users);

            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.SenderId);
                entity.Property(x => x.RecipientId);

                entity.Property(e => e.Text)
                    .HasMaxLength(1000);

                entity.HasOne(x => x.Sender)
                    .WithMany(x => x.SendMessages)
                    .HasForeignKey(x => x.SenderId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Recipient)
                    .WithMany(x => x.ReceiveMessages)
                    .HasForeignKey(x => x.RecipientId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
