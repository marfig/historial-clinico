using HistorialClinico.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface ICamaService
    {
        Task MovimientoCamaPacienteAsync(string TipoMovimientoId, int CamaId, int PacienteId, string user_name);

        Task<List<CamaPaciente>> GetCamasPacientesAsync();
    }
}
