using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface IApCardiovascularService
    {
        Task<List<ApCardiovascularDTO>> ListApCardiovascularAsync(int PacienteId);
        Task<List<Inotropico>> ListInotropicosAsync(int ApCardiovascularId, bool Relacionados);
        Task<List<EnzimaCardiaca>> ListEnzimasCardiacasAsync(int ApCardiovascularId);
        Task AddApCardiovascularAsync(int PacienteId, string EstadoId, string EvaluacionCardiologica, string InotropicosJSON, string EnzimasCardiacasJSON, string Eventos, string Planes, string user_name);
    }
}
