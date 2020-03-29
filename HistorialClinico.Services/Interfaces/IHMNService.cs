using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using HistorialClinico.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface IHMNService
    {
        Task<List<GeneralHMN>> ListGeneralHMNAsync();
        Task<List<BalanceHidricoHMN>> ListBalanceHidricoHMNAsync();
        Task<List<LaboratorioHMN>> ListLaboratorioHMNAsync();
        Task AddHMNAsync(HmnDTO model);
        Task<List<HMN>> ListHMNAsync(int PacienteId);
        Task<List<ListHMN>> HMNGeneralAsync(int HMNId);
        Task<List<ListHMN>> HMNBalanceHidricoHMNlAsync(int HMNId);
        Task<List<ListHMN>> HMNLaboratorioHMNAsync(int HMNId);
    }
}
