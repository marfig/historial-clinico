namespace HistorialClinico.Domain.DTO
{
    public class InfectologicoDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int EstadoInfectologicoId { get; set; }
        public string CoberturaAtbParamJSON { get; set; }
        public string CultivoParamJSON { get; set; }
        public string SensibilidadParamJSON { get; set; }
        public string HisopadoParamJSON { get; set; }
        public string Interconsulta { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
    }
}
