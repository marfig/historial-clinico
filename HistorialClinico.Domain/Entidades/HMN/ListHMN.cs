using System;

namespace HistorialClinico.Domain.Entidades
{
    public class ListHMN
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Valor { get; set; }
        public string Formulacion { get; set; }
        public string Categoria { get; set; }
    }
}