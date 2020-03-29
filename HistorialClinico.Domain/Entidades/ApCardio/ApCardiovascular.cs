using System;

namespace HistorialClinico.Domain
{
    public class ApCardiovascular
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string EstadoId { get; set; }
        public string EvaluacionCardiologica { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserAdd { get; set; }
        public DateTime DateAdd { get; set; }
    }
}