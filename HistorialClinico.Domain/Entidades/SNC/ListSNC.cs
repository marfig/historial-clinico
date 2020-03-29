using System;

namespace HistorialClinico.Domain.Entidades
{
    public class ListSNC
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? Valor { get; set; }
    }
}