using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialEngineeringForum.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }

        [Required(ErrorMessage = "Сообщение не может быть пустым.")]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public bool IsEdited { get; set; } = false;

        public DateTime? EditDate { get; set; }
    }
}