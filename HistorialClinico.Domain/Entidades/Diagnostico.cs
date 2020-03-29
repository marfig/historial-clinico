using System;

namespace HistorialClinico.Domain
{
    public class Diagnostico
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public DateTime FechaAdd { get; set; }
        public string UserNameAdd { get; set; }
        public string Resumen { get; set; }
    }
}