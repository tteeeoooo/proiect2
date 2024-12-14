using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Store.Models;

namespace Store.Models
{

    public class Product
    {
        [Key]
        //[MaxLength(255)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The product name is required")]
        [StringLength(100, ErrorMessage = "The product name cannot exceed 100 characters.")]
        public string Name { get; set; }
// name stock description ingredients price brand categoryId (virtual category)
//required: name, stock, ingredients, price, category

        [Required(ErrorMessage = "The stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The stock cannot be negative.")]
        public int Stock { get; set; }

        [StringLength(1000, ErrorMessage = "The description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This substance is not allowed in any of our products")]
        [MaxLength(1000)]
        [ForbiddenWords("Alcohol", "Bithionol", "Chloroform", "metabromsalan", "tetrachlorosalicylanilide",
        "Hexachlorophene", "Mercury compounds", "Methylene chloride", "Vinyl chloride", "Zirconium-containing",
        "Parabens", "Polyethylene glycol", "Butylated hydroxytoluene", "Hydroquinone", "Petroleum", 
        "Butylated hydroxytoluene", "Lead", "Fragrance")]
        public string Ingredients { get; set; }

        [Required(ErrorMessage = "The product is not for free")]
        [Range(5, 250, ErrorMessage = "We are not selling things for free and we are also not selling gold")]
        public float Price { get; set; }

        public DateTime DateListed { get; set; } = DateTime.Now;

        public string Brand { get; set; }

        [Required(ErrorMessage = "Adding the product category is required")]
        public int CategoryId { get; set; } = -1;
        public virtual Category Category { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; } 
    }
}