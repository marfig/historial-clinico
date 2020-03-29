using HistorialClinico.Domain.DTO;
using HistorialClinico.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface IApRespiratorioService
    {
        Task<List<ApRespiratorio>> ListApRespiratorioAsync(int PacienteId);
        Task<List<SoporteRespiratorio>> ListSoporteRespiratorioAsync();
        Task<List<SoporteRespParametros>> ListSoporteRespParametrosAsync(int ApRespiratorioId);
        Task<List<Ventilacion>> ListVentilacionAsync();
        Task<List<Modalidad>> ListModalidadAsync();
        Task<List<Gasometria>> ListGasometriaAsync();
        Task<List<GasometriaParametros>> ListGasometriaParametrosAsync(int ApRespiratorioId);
        Task AddApRespiratorioAsync(ApRespiratorioDTO model);
    }
}