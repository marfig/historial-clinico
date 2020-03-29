using System;

namespace HistorialClinico.Domain.DTO
{
    public class DiagnosticoDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string Resumen { get; set; }
        public string UserName { get; set; }
        public DateTime Fecha { get; set; }
    }
}