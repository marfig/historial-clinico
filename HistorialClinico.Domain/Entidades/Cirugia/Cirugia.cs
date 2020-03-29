using System;

namespace HistorialClinico.Domain
{
    public class Cirugia
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public bool TuvoCirugia { get; set; }
        public string Tecnica { get; set; }
        public string Hallazgos { get; set; }
        public bool Procedimiento { get; set; }
        public string CualProcedimiento { get; set; }
        public string OtrasAcotaciones { get; set; }
        public string UserAdd { get; set; }
        public DateTime DateAdd { get; set; }
        public bool Deleted { get; set; }
    }
}
