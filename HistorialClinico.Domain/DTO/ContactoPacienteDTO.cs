namespace HistorialClinico.Domain.DTO
{
    public class ContactoPacienteDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string TipoContactoId { get; set; }
        public string NroContacto { get; set; }
        public string NombreContacto { get; set; }
        public string UserName { get; set; }
    }
}