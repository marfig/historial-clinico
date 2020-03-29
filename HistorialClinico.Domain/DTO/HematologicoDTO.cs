using System;

namespace HistorialClinico.Domain.DTO
{
    public class HematologicoDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public decimal? Hemograma_PAI { get; set; }
        public decimal? Hemograma_HB { get; set; }
        public decimal? Hemograma_HTC { get; set; }
        public decimal? Hemograma_PLT { get; set; }
        public decimal? Crasis_TP { get; set; }
        public decimal? Crasis_TTPA { get; set; }
        public decimal? Crasis_Fibrinoginos { get; set; }
        public bool VitaminaK { get; set; }
        public decimal? DosisVitaminaK { get; set; }
        public DateTime? FechaVitaminaK { get; set; }
        public bool SangradoActivo { get; set; }
        public string LugarSangrado { get; set; }
        public bool Transfusiones { get; set; }
        public decimal? Transfusiones_GRC { get; set; }
        public decimal? Transfusiones_PFC { get; set; }
        public decimal? Transfusiones_CRIO { get; set; }
        public decimal? Transfusiones_PLT { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
    }
}