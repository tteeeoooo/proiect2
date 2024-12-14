using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models;
using System.Security.Claims;
namespace Store.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext db;

        public ShoppingCartController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            var cart = db.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View(new ShoppingCart()); 
            }

            return View(cart); 
        }

        [HttpPost]
        [Authorize(Roles = "Inregistrat,Colaborator,Administrator")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            // if (!User.Identity.IsAuthenticated)
            // {
            //     return RedirectToRoute("/Identity/Account/Register"); 
            // }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (userId == null)
            {
                return Unauthorized(); 
            }

            var cart = GetCart(userId);

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            db.SaveChanges(); 
            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = GetCart(userId);

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = GetCart(userId);

            db.CartItems.RemoveRange(cart.Items);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private ShoppingCart GetCart(string userId)
        {
            var cart = db.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                db.ShoppingCarts.Add(cart);
                db.SaveChanges();
            }

            return cart;
        }


        [HttpPost]
        [Authorize]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var cart = db.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (cartItem != null)
                {
                    if (quantity > 0)
                    {
                        cartItem.Quantity = quantity;
                    }
                    else
                    {
                        cart.Items.Remove(cartItem);
                    }

                    db.SaveChanges(); 
                }
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize]
        public IActionResult PlaceOrder()
        {
            Console.WriteLine("PlaceOrder called");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = db.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
            {
                Console.WriteLine("Cart is empty or null");
                return RedirectToAction("Index", "ShoppingCart");
            }

            var order = new ShoppingHistory
            {
                UserId = userId,
                PurchaseDate = DateTime.Now,
                TotalAmount = cart.Items.Sum(i => i.Quantity * (decimal)i.Product.Price),
                Items = cart.Items.Select(i => new ShoppingHistoryItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = (decimal)i.Product.Price
                }).ToList()
            };

            db.ShoppingHistories.Add(order);
            db.CartItems.RemoveRange(cart.Items);
            db.SaveChanges();

            Console.WriteLine($"Order created with ID: {order.Id}");

            Console.WriteLine($"Order placed successfully with ID: {order.Id}");
            
            Console.WriteLine($"Redirecting to OrderConfirmation with ID: {order.Id}");
            return RedirectToAction("OrderConfirmation", "ShoppingHistory", new { id = order.Id });
        }



        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = db.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                ShoppingCart = cart,
                TotalAmount = (decimal)cart.Items.Sum(i => i.Quantity * i.Product.Price)
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = db.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "ShoppingCart"); 
            }

            var order = new ShoppingHistory
            {
                UserId = userId,
                PurchaseDate = DateTime.Now,
                TotalAmount = (decimal)cart.Items.Sum(i => i.Product.Price * i.Quantity),
                Items = cart.Items.Select(i => new ShoppingHistoryItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = (decimal)i.Product.Price
                }).ToList()
            };

            db.ShoppingHistories.Add(order);
            db.CartItems.RemoveRange(cart.Items); 
            db.SaveChanges();

            return RedirectToAction("OrderConfirmation", "ShoppingHistory", new { id = order.Id });
        }
    }
}