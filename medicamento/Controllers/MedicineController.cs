using medicamento.Models.Statistics;
using medicamento.Services.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medicamento.Controllers
{
    public class MedicineController : Controller
    {
        private readonly ILogger<MedicineController> _logger;
        private readonly AppDbContext _context;

        public MedicineController(ILogger<MedicineController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Route("Medicine/List/{param?}")]
        public async Task<IActionResult> List(string param)
        {
            var medicamento = await _context.Prescripcion
                .Where(m => m.des_comercial.Contains(param) || m.principios_activos.Contains(param) || m.cod_definitivo == param)
                .ToListAsync();
            return View(medicamento);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicamento = await _context.Prescripcion
                .FirstOrDefaultAsync(m => m.cod_nacional == id);

            if (medicamento == null)
            {
                return NotFound();
            }

            StatisticsModel statistics = new StatisticsModel
            {
                user_id = User.Identity.Name,
                key = "medicine",
                date = DateTime.Now,
                nro_registro = id,
                type = 1
            };

            _context.Statistics.Add(statistics);
            await _context.SaveChangesAsync();

            return View(medicamento);
        }

    }
}
