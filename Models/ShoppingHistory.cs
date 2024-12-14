using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public class ShoppingHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } 

        [Required]
        public decimal TotalAmount { get; set; } 

        public virtual ICollection<ShoppingHistoryItem> Items { get; set; } = new List<ShoppingHistoryItem>();
    }
}