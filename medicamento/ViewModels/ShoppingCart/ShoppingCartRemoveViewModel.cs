namespace medicamento.ViewModels.ShoppingCart
{
    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; } = string.Empty;
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}
