namespace HistorialClinico.Domain.DTO
{
    public class SncDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string AspectoGralJSON { get; set; }
        public int SedacionId { get; set; }
        public decimal ValorSedacion { get; set; }
        public string SedacionMedicamentoJSON { get; set; }
        public string LaboratorioJSON { get; set; }
        public string ImagenesJSON { get; set; }
        public bool SxAbstinenciaId { get; set; }
        public string SxAbstinenciaJSON { get; set; }
        public string SxAbstinenciaMedicacionJSON { get; set; }
        public bool ConocidoConvulsionadorId { get; set; }
        public string ConocidoConvulsionadorJSON { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
    }
}