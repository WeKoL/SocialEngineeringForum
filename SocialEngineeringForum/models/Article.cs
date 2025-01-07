using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialEngineeringForum.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок статьи обязателен.")]
        [StringLength(250, ErrorMessage = "Заголовок статьи не должен превышать 250 символов.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержание статьи обязательно.")]
        public string Content { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }
    }
}