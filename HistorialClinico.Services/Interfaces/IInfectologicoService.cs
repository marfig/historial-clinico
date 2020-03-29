using HistorialClinico.Domain.DTO;
using HistorialClinico.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface IInfectologicoService
    {
        Task<List<Infectologico>> ListInfectologicoAsync(int PacienteId);
        Task<List<InfectologicoCoberturaAtb>> InfectologicoCoberturaAtbAsync(int InfectologicoId);
        Task<List<InfectologicoCultivo>> InfectologicoCultivoAsync(int InfectologicoId);
        Task<List<SensibilidadCultivoInfectologico>> SensibilidadCultivoInfectologicoAsync(int InfectologicoId, int CultivoId);
        Task<List<HisopadoInfectologico>> HisopadoInfectologicoAsync(int InfectologicoId);
        Task<List<EstadoInfectologico>> ListEstadoInfectologicoAsync();
        Task<List<Cultivo>> ListCultivoAsync();
        Task<List<Hisopado>> ListHisopadoAsync();
        Task AddInfectologicoAsync(InfectologicoDTO model);
    }
}