using System.ComponentModel.DataAnnotations;
using Store.Models;

namespace Store.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The category name must be specified")]
        [StringLength(100, ErrorMessage = "The category name cannot exceed 100 characters.")]
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}