using System.ComponentModel.DataAnnotations;
using medicamento.Models.Medicine;

namespace medicamento.Models.ShoppingCart
{
    public class OrderDetailModel
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string NroRegistro { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual PrescripcionModel? Medicine { get; set; } = new();
        public virtual OrderModel? Order { get; set; }

        public decimal TotalPrice()
        {
            return UnitPrice * Quantity;
        }
    }
}
