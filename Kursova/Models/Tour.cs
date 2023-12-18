using Kursova.Models.Categories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursova.Models
{
    public class Tour
    {
        public int TourId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TourName { get; set; }
        public string TourDescription { get; set; }
        public string Image { get; set; }

        public double Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CardDetails { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
    }
}
