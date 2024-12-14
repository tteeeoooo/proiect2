using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models;
using System.Linq;
using System.Security.Claims;

namespace Store.Controllers
{
    [Authorize]
    public class ShoppingHistoryController : Controller
    {
        private readonly ApplicationDbContext db;

        public ShoppingHistoryController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = db.ShoppingHistories
                .Include(h => h.Items)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.PurchaseDate)
                .ToList();

            return View(orders);
        }

        [HttpGet]
        public IActionResult OrderConfirmation(int id)
        {
            Console.WriteLine($"OrderConfirmation called with ID: {id}");
            var order = db.ShoppingHistories
                .Include(h => h.Items)
                .FirstOrDefault(h => h.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order); 
        }
    }
}