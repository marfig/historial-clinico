using System;

namespace HistorialClinico.Domain.Entidades
{
    public class InfectologicoCoberturaAtb
    {
        public int Id { get; set; }
        public string Antibiotico { get; set; }
        public decimal Dosis { get; set; }
        public string Unidad { get; set; }
        public bool AjustadoClearence { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaSuspension { get; set; }
    }
}
