using System;

namespace HistorialClinico.Domain
{
    public class ContactoPaciente
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string TipoContactoId { get; set; }
        public string NombreContacto { get; set; }
        public string NroContacto { get; set; }
    }
}