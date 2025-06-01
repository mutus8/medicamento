using System.ComponentModel.DataAnnotations;

namespace medicamento.Models.Medicine
{
    public class PrescripcionModel
    {
        [Key]
        public string cod_nacional { get; set; } = string.Empty;
        public string cod_definitivo { get; set; } = string.Empty;
        public string des_comercial { get; set; } = string.Empty;
        public string des_presentacion { get; set; } = string.Empty;
        public string url_ficha { get; set; } = string.Empty;
        public string url_prospecto { get; set; } = string.Empty;
        public string img_envase { get; set; } = string.Empty;
        public string img_forma { get; set; } = string.Empty;
        public bool receta { get; set; }
        public bool comercializado { get; set; }
        public string laboratorio { get; set; } = string.Empty;
        public DateTime fecha_autorizacion { get; set; }
        public DateTime fecha_comercializado { get; set; }
        public string principios_activos { get; set; } = string.Empty;
        public string excipientes { get; set; } = string.Empty;
        public decimal precio { get; set; }
    }
}
