using System;

namespace HistorialClinico.Domain
{
    public class CamaPaciente
    {
        public int Id { get; set; }
        public string Cama { get; set; }
        public int? PacienteId { get; set; }
        public string NombrePaciente { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaIngreso { get; set; }
    }
}