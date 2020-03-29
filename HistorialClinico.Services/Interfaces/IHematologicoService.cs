using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface IHematologicoService
    {
        Task AddHematologicoAsync(HematologicoDTO model);
        Task<List<Hematologico>> ListHematologicoAsync(int PacienteId);
    }
}
