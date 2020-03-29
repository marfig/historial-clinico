using System;
using System.Collections.Generic;

namespace HistorialClinico.Web.Models.Paciente
{
    public class PacienteGridModel
    {
        public int Id { get; set; }
        public string Cama { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Edad { get; set; }
    }

    public class PacienteFormModel
    {
        public int Id { get; set; }
        public int CamaId { get; set; }
        public string NroDocumento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sexo { get; set; }
        public string FechaNacimiento { get; set; }
        public string Peso { get; set; }
        public string GrupoSanguineo { get; set; }
        public List<ContactoPacienteFormModel> Contactos { get; set; }
        public string ContactosJSON { get; set; }
        public int? PrmsId { get; set; }
    }

    public class ContactoPacienteFormModel
    {
        public int ContactoId { get; set; }
        public string TipoContactoId { get; set; }
        public string NroContacto { get; set; }
        public string NombreContacto { get; set; }
    }

    public class DiagnosticoFormModel
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string Resumen { get; set; }
    }
    public class DiagnosticoGridModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Resumen { get; set; }
        public string Usuario { get; set; }
    }
}
