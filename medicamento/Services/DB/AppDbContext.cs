using medicamento.Models.Account;
using medicamento.Models.Medicine;
using medicamento.Models.ShoppingCart;
using medicamento.Models.Statistics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace medicamento.Services.DB
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PrescripcionModel> Prescripcion { get; set; }
        public DbSet<StatisticsModel> Statistics { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
    }
}
