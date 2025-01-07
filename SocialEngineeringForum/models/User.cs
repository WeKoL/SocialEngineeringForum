using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialEngineeringForum.Models
{
    public enum UserType
    {
        Admin,
        Premium,
        Regular,
        Guest
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Указываем, что ID будет генерироваться автоматически
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя пользователя обязательно.")]
        [StringLength(50, ErrorMessage = "Имя пользователя не должно превышать 50 символов.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Хэш пароля обязателен.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный email.")]
        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginDate { get; set; }

        public UserType UserType { get; set; }

        [StringLength(200)]
        public string? AvatarUrl { get; set; }

        public int Points { get; set; } = 0;

        [StringLength(1000)]
        public string? Bio { get; set; }

        public bool IsBanned { get; set; } = false;

        // Навигационные свойства
        public ICollection<Topic> Topics { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}