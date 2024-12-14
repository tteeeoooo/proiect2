using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public class ShoppingHistoryItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ShoppingHistory")]
        public int ShoppingHistoryId { get; set; }
        public virtual ShoppingHistory ShoppingHistory { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; } 
    }
}