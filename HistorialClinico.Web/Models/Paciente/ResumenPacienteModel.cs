using System;

namespace HistorialClinico.Web.Models.Paciente
{
    public class ResumenPacienteModel
    {
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string GrupoSanguineo { get; set; }
        public string Edad { get; set; }
    }
}
