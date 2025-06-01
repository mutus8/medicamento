using medicamento.Models.Medicine;
using medicamento.Models.ShoppingCart;
using medicamento.Services.DB;
using Microsoft.AspNetCore.Mvc;

namespace medicamento.Services.ShoppingCart
{
    public partial class ShoppingCart
    {
        public const string CartSessionKey = "CartId";
        string ShoppingCartId { get; set; }

        private readonly AppDbContext _context;

        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }

        public static ShoppingCart GetCart(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var dbContext = context.RequestServices.GetService<AppDbContext>();
            if (dbContext == null) throw new InvalidOperationException("AppDbContext service is not available.");

            var cart = new ShoppingCart(dbContext);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(PrescripcionModel album)
        {
            
            //var cartItem = _context.Carts.SingleOrDefault(
            //    c => c.CartId == ShoppingCartId
            //    && c.NroRegistro == album.nro_registro);
            var cartItem = _context.Carts?.FirstOrDefault(
                c => c.CartId == ShoppingCartId
                && c.CodNacional == album.cod_nacional);

            if (cartItem == null)
            {
                cartItem = new CartModel
                {
                    CodNacional = album.cod_nacional,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                    Medicine = album,
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
           
            _context.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            var cartItem = _context.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _context.Carts.Remove(cartItem);
                }
                _context.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = _context.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _context.Carts.Remove(cartItem);
            }
            _context.SaveChanges();
        }
        public List<CartModel> GetCartItems()
        {
            List<CartModel> cartItems = _context.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();

            foreach (var cartItem in cartItems)
            {
                cartItem.Medicine = _context.Prescripcion.FirstOrDefault(m => m.cod_nacional == cartItem.CodNacional);
            }
            return cartItems;
        }
        public int GetCount()
        {
            int? count = (from cartItems in _context.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }
        public decimal GetTotal()
        {
            decimal? total = (from cartItems in _context.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Medicine.precio).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(OrderModel order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            
            foreach (var item in cartItems)
            {
                var medicine = _context.Prescripcion
                    .FirstOrDefault(m => m.cod_nacional == item.CodNacional);

                var orderDetail = new OrderDetailModel
                {
                    NroRegistro = item.CodNacional,
                    OrderId = order.OrderId,
                    UnitPrice = item.Medicine.precio,
                    Quantity = item.Count,
                    Medicine = medicine,
                };
                
                orderTotal += (item.Count * item.Medicine.precio);

                _context.OrderDetails.Add(orderDetail);

            }
            
            order.Total = orderTotal;

            _context.SaveChanges();
            
            EmptyCart();
            
            return order.OrderId;
        }
        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            return context.Session.GetString(CartSessionKey);
        }
        
        public void MigrateCart(string userName)
        {
            var shoppingCart = _context.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (CartModel item in shoppingCart)
            {
                item.CartId = userName;
            }
            _context.SaveChanges();
        }
    }
}
