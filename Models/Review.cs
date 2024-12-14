using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Store.Models;

namespace Store.Models
{
    public class Review
    {
        [Key]
        //[MaxLength(255)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Must enter the review content")]
        [ForbiddenWords("fuck", "hell", "ew", "eww", "shit", "wtf", "hell")]
        public string Content { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Value should be a number between 1 and 5")]
        public int Grade {get; set;}
        public DateTime Date { get; set; }
        // [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}