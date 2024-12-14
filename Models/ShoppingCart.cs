using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Store.Models;

namespace Store.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}