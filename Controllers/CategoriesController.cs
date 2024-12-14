using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models;
using System.Linq;

namespace Store.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;
        public CategoriesController(ApplicationDbContext context)
        {
            db = context;
        }





        [HttpGet]
        [Authorize(Roles = "Colaborator,Administrator")]
        public async Task<IActionResult> IndexAsync()
        {
            var categories = await db.Categories.ToListAsync();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult New(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category); 
            }

            try
            {
                db.Categories.Add(category); 
                db.SaveChanges();
                TempData["Success"] = "Category created successfully!";
                return RedirectToAction("Index", "Categories");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the category: " + ex.Message);
                return View(category); // Revino la formular dacă apare o eroare
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                db.Categories.Update(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Caută categoria în baza de date
            var category = await db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Category '{category.CategoryName}' and all of its products have been deleted";
            return RedirectToAction(nameof(IndexAsync));
        }


    }
}


