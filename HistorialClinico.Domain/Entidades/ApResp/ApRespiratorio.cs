using System;

namespace HistorialClinico.Domain.Entidades
{
    public class ApRespiratorio
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int? SoporteRespiratorioId { get; set; }
        public string SoporteRespiratorio { get; set; }
        public bool Parametros { get; set; }
        public string ValorSoporteResp { get; set; }
        public int? VentilacionId { get; set; }
        public string Ventilacion { get; set; }
        public int? ModalidadId { get; set; }
        public string Modalidad { get; set; }
        public int? GasometriaId { get; set; }
        public string Gasometria { get; set; }
        public string Manejo { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserAdd { get; set; }
        public DateTime DateAdd { get; set; }
        public bool Deleted { get; set; }
    }
}