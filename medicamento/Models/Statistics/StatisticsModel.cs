namespace medicamento.Models.Statistics
{
    public class StatisticsModel
    {
        public int id { get; set; }
        public string user_id { get; set; } = string.Empty;
        public string key { get; set; } = string.Empty;
        public DateTime date { get; set; }
        public string nro_registro { get; set; } = string.Empty;
        public int type { get; set; }
    }
}
