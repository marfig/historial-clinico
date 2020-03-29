namespace HistorialClinico.Web.Models.Paciente
{
    public class CamaPacienteGridModel
    {
        public int Id { get; set; }
        public string Cama { get; set; }
        public int? PacienteId { get; set; }
        public string NombrePaciente { get; set; }
        public string Edad { get; set; }
        public int DiasInternacion { get; set; }
    }
}
