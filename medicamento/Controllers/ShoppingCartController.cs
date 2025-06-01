using medicamento.Services.DB;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using medicamento.Services.ShoppingCart;
using medicamento.ViewModels.ShoppingCart;

namespace medicamento.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<MedicineController> _logger;
        private readonly AppDbContext _context;
        public ShoppingCartController(ILogger<MedicineController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public ActionResult Index()
        {
            var pathBase = HttpContext;
            var cart = ShoppingCart.GetCart(pathBase);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }

        public ActionResult AddToCart(string id)
        {
            var addedMedicine = _context.Prescripcion
                .Single(album => album.cod_nacional == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedMedicine);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string medicineName = _context.Carts.Single(item => item.RecordId == id).Medicine.des_comercial;

            int itemCount = cart.RemoveFromCart(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = HttpUtility.HtmlEncode(medicineName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                //CartTotal = 0,
                //CartCount = 0,
                //ItemCount = 0,
                DeleteId = id
            };
            return Json(results);
        }

        [System.Web.Mvc.ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("~/Views/Shared/_CartSummaryPartial");
        }
    }
}
