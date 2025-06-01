using medicamento.Models.Medicine;
using System.ComponentModel.DataAnnotations;

namespace medicamento.Models.ShoppingCart
{
    public class CartModel
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; } = string.Empty;
        public string CodNacional { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual PrescripcionModel Medicine { get; set; } = new();
    }
}
