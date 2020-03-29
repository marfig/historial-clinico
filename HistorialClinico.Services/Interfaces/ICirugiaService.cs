using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface ICirugiaService
    {
        Task AddCirugiaAsync(CirugiaDTO model);
        Task<List<Cirugia>> ListCirugiaAsync(int PacienteId);
    }
}
