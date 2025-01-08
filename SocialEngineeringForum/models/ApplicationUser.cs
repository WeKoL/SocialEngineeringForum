using Microsoft.AspNetCore.Identity;
namespace SocialEngineeringForum.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Добавь нужные тебе поля из класса User, например:
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public int Points { get; set; }
        public bool IsBanned { get; set; } = false;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginDate { get; set; }
        // ... другие поля
    }
}