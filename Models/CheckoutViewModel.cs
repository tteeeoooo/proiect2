using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class CheckoutViewModel
    {
        public ShoppingCart ShoppingCart { get; set; } 

        [Required(ErrorMessage = "Shipping address is required.")]
        [StringLength(200, ErrorMessage = "The shipping address cannot exceed 200 characters.")]
        public string ShippingAddress { get; set; } 

        [Required(ErrorMessage = "Cardholder name is required.")]
        [StringLength(100, ErrorMessage = "The cardholder name cannot exceed 100 characters.")]
        public string CardholderName { get; set; } 

        [Required(ErrorMessage = "Card number is required.")]
        [CreditCard(ErrorMessage = "Invalid credit card number.")]
        public string CardNumber { get; set; } 

        [Required(ErrorMessage = "Expiration date is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Expiration date must be in MM/YY format.")]
        public string ExpirationDate { get; set; } 

        [Required(ErrorMessage = "CVV is required.")]
        [StringLength(3, ErrorMessage = "CVV must be 3 digits.")]
        public string CVV { get; set; } 

        public decimal TotalAmount { get; set; } 

        public int ShoppingHistoryId { get; set; } 
    }
}