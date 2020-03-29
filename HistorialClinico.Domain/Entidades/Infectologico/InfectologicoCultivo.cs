using System;

namespace HistorialClinico.Domain.Entidades
{
    public class InfectologicoCultivo
    {
        public int CultivoId { get; set; }
        public string Cultivo { get; set; }
        public bool Resultado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
