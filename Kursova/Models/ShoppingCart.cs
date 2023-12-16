using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursova.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }

    
        [Required]
        public string UserId { get; set; }

        public bool isDeleted { get; set; } = false;

        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
