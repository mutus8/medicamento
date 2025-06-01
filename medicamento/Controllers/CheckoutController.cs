using medicamento.Models.ShoppingCart;
using medicamento.Services.DB;
using medicamento.Services.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;

namespace medicamento.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly AppDbContext _context;
        public CheckoutController(ILogger<CheckoutController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        const string PromoCode = "FREE";
        
        public ActionResult AddressAndPayment()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(IFormCollection values)
        {
            var order = new OrderModel();
            await TryUpdateModelAsync(order);

            try
            {
                //if (string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase) == false)
                //{
                //    return View(order);
                //}
                //else
                //{
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                // Save Order  
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Process the order  
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.CreateOrder(order);

                return RedirectToAction("Complete", new { id = order.OrderId });
                //}
            }
            catch
            {
                return View(order);
            }
        }

        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = _context.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
