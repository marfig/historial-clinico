using System;

namespace HistorialClinico.Domain.DTO
{
    public class CultivoDTO
    {
        public int CultivoId { get; set; }
        public bool Resultado { get; set; }
        public string Fecha { get; set; }
    }
}