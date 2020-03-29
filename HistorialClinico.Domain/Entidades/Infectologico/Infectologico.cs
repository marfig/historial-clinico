using System;

namespace HistorialClinico.Domain.Entidades
{
    public class Infectologico
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int EstadoInfectologicoId { get; set; }
        public string EstadoInfectologico { get; set; }
        public string Interconsulta { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserAdd { get; set; }
        public DateTime DateAdd { get; set; }
        public bool Deleted { get; set; }
    }
}