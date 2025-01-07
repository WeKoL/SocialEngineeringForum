using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialEngineeringForum.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название категории обязательно.")]
        [StringLength(100, ErrorMessage = "Название категории не должно превышать 100 символов.")]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Навигационные свойства
        public ICollection<Topic> Topics { get; set; }
    }
}