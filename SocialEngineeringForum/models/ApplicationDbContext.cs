using Microsoft.EntityFrameworkCore;
using SocialEngineeringForum.Models; //Убедись, что имя проекта тут корректное

namespace SocialEngineeringForum.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связей и каскадного удаления
            modelBuilder.Entity<Topic>()
             .HasOne(t => t.Author)
             .WithMany(u => u.Topics)
             .HasForeignKey(t => t.AuthorId)
             .OnDelete(DeleteBehavior.NoAction); //или .Restrict

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Author)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Topic)
                .WithMany(t => t.Messages)
                .HasForeignKey(m => m.TopicId)
                .OnDelete(DeleteBehavior.Cascade); // разрешаем каскадное удаление только тут

            modelBuilder.Entity<Article>()
               .HasOne(a => a.Author)
               .WithMany(u => u.Articles)
               .HasForeignKey(a => a.AuthorId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}