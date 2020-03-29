using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using HistorialClinico.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface ISNCService
    {
        Task<List<AspectoGeneralSNC>> ListAspectoGralAsync();
        Task<List<Sedacion>> ListSedacionAsync();
        Task<List<MedicamentoSedacion>> ListMedicamentoSedacionAsync();
        Task<List<LaboratorioSNC>> ListLaboratorioSNCAsync();
        Task<List<ImagenSNC>> ListImagenSNCAsync();
        Task<List<SxAbstinenciaSNC>> ListSxAbstinenciaSNCAsync();
        Task<List<MedicacionSNC>> ListMedicacionSNCAsync();
        Task<List<Anticonvulsionante>> ListAnticonvulsionanteAsync();
        Task AddSNCAsync(SncDTO model);
        Task<List<SNC>> ListSNCAsync(int PacienteId);
        Task<List<ListSNC>> SNCAspectoGralAsync(int SNCId);
        Task<List<ListSNC>> SNCMedicamentoSedacionAsync(int SNCId);
        Task<List<ListSNC>> SNCLaboratorioAsync(int SNCId);
        Task<List<ListSNC>> SNCImagenesAsync(int SNCId);
        Task<List<ListSNC>> SNCAnticonvulsionanteAsync(int SNCId);
        Task<List<ListSNC>> SNCSxAbstinenciaAsync(int SNCId);
        Task<List<ListSNC>> SNCAbstinenciaMedicacionAsync(int SNCId);
    }
}
