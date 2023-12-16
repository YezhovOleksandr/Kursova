﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursova.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int CartDetailId { get; set; }
        [Required]
        public int ShoppingCartId {  get; set; }
        [Required]

        public int Quantity { get; set; }
        public int TourId { get; set; }

        public Tour tour { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
