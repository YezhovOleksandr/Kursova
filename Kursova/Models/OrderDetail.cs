using System.ComponentModel.DataAnnotations.Schema;

namespace Kursova.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }

        public int TourId { get; set; }
        public double UnitPrice { get; set; }
        public Order Order { get; set; }

        public Tour tour { get; set; }
    }
}
