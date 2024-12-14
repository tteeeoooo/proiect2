using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReviewsController(ApplicationDbContext context)
        {
            db = context;
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Index()
        {
            var reviews = db.Reviews.ToList();
            return View(reviews);
        }

        // Detaliile unei recenzii
        public IActionResult Details(int id)
        {
            var product = db.Products
                .Include(p => p.Reviews) 
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Crearea unei recenzii (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Crearea unei recenzii (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.Date = DateTime.Now; 
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // Editarea unei recenzii (GET)
        public IActionResult Edit(int id)
        {
            var review = db.Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // Editarea unei recenzii (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Update(review);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // Ștergerea unei recenzii (GET)
        public IActionResult Delete(int id)
        {
            var review = db.Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // Ștergerea unei recenzii (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = db.Reviews.FirstOrDefault(r => r.Id == id);
            if (review != null)
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(Review review)
        {
            Console.WriteLine("AddReview method called.");
            Console.WriteLine($"ProductId: {review.ProductId}");
            Console.WriteLine($"Content: {review.Content}");
            Console.WriteLine($"Grade: {review.Grade}");

            review.Date = DateTime.Now; 
            db.Reviews.Add(review); 
            db.SaveChanges(); 
            Console.WriteLine("Review saved to database.");

            return RedirectToAction("Details", "Products", new { id = review.ProductId });
        }
    }
}