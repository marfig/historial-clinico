namespace HistorialClinico.Domain.DTO
{
    public class HmnDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public bool DialisisPeritoneal { get; set; }
        public string FormulacionDialisisPeritoneal { get; set; }
        public string GeneralJSON { get; set; }
        public string BalanceHidricoJSON { get; set; }
        public string LaboratorioJSON { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
    }
}