namespace HistorialClinico.Domain.DTO
{
    public class CoberturaAtbDTO
    {
        public string Antibiotico { get; set; }
        public string Dosis { get; set; }
        public string Unidad { get; set; }
        public bool AjustadoClearence { get; set; }
        public string FechaInicio { get; set; }
        public string FechaSuspension { get; set; }
    }
}
