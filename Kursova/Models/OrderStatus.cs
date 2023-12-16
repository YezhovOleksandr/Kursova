using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursova.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int OrderStatusId { get; set; }

        [Required]
        public int StatusId { get; set; }
        [Required]
        [MaxLength(20)]
        public string OrderStatusName { get; set;}

    }
}
