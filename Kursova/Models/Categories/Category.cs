using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursova.Models.Categories
{
    [Table("Category")]
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        public List<Tour> Tours { get; set; }
    }
}
