using medicamento.Models.ShoppingCart;

namespace medicamento.ViewModels.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public List<CartModel> CartItems { get; set; } = [];
        public decimal CartTotal { get; set; }
    }
}
