using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialEngineeringForum.Models
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название темы обязательно.")]
        [StringLength(200, ErrorMessage = "Название темы не должно превышать 200 символов.")]
        public string Title { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public DateTime LastActivityDate { get; set; } = DateTime.UtcNow;

        public bool IsClosed { get; set; } = false;

        // Навигационные свойства
        public ICollection<Message> Messages { get; set; }
    }
}