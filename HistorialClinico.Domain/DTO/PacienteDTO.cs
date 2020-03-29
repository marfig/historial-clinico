using System;

namespace HistorialClinico.Domain.DTO
{
    public class PacienteDTO
    {
        public int? Id { get; set; }
        public string NroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string GrupoSanguineo { get; set; }
        public decimal? Peso { get; set; }
        public int? PrmsId { get; set; }
        public int? CamaId { get; set; }
        public string UserName { get; set; }
    }
}