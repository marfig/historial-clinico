using System;

namespace HistorialClinico.Domain.Entidades
{
    public class SNC
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int SedacionId { get; set; }
        public string Sedacion { get; set; }
        public decimal ValorSedacion { get; set; }
        public bool SxAbstinencia { get; set; }
        public bool ConocidoConvulsionador { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserAdd { get; set; }
        public DateTime DateAdd { get; set; }
        public bool Deleted { get; set; }
    }
}