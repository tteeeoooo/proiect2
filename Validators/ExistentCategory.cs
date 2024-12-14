// using System.ComponentModel.DataAnnotations;
// using Microsoft.EntityFrameworkCore;
// using Store.Data; 

// public class ValidCategoryAttribute : ValidationAttribute
// {
//     protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//     {
//         if (value == null)
//         {
//             return new ValidationResult("Categoria este obligatorie.");
//         }

//         var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
//         if (dbContext == null)
//         {
//             throw new InvalidOperationException("ApplicationDbContext nu este configurat corect în DI.");
//         }

//         var categoryId = (int)value;
//         var categoryExists = dbContext.Categories.Any(c => c.Id == categoryId);

//         if (!categoryExists)
//         {
//             return new ValidationResult("Categoria specificată nu există în baza de date.");
//         }

//         return ValidationResult.Success;
//     }
// }