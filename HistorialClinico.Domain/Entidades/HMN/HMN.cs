using System;

namespace HistorialClinico.Domain.Entidades
{
    public class HMN
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public bool DialisisPeritoneal { get; set; }
        public string FormulacionDialisisPeritoneal { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserAdd { get; set; }
        public DateTime DateAdd { get; set; }
        public bool Deleted { get; set; }
    }
}