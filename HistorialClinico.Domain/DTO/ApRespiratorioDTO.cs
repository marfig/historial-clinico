namespace HistorialClinico.Domain.DTO
{
    public class ApRespiratorioDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int? SoporteRespiratorioId { get; set; }
        public string ValorSoporteResp { get; set; }
        public string SoporteRespiratorioParamJSON { get; set; }
        public int? VentilacionId { get; set; }
        public int? ModalidadId { get; set; }
        public int? GasometriaId { get; set; }
        public string GasometriaParamJSON { get; set; }
        public string Manejo { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
    }
}
