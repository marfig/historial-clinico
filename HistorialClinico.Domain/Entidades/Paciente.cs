using System;

namespace HistorialClinico.Domain
{
    public class Paciente
    {
        public int Id { get; set; }
        public string NroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string GrupoSanguineo { get; set; }
        public decimal? Peso { get; set; }
        public int? PrmsId { get; set; }
    }
}