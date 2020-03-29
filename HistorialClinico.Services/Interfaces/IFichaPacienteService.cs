using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface IFichaPacienteService
    {
        Task<IEnumerable<PacienteDTO>> ListarPacientesAsync(string Valor);

        Task<PacienteDTO> AddEditDatosBasicosPacienteAsync(PacienteDTO item);

        Task AddEditContactoPacienteAsync(IEnumerable<ContactoPacienteDTO> contactos);

        Task DeleteContactoPacienteAsync(int ContactoId, string user_name);

        Task AddEditDiagnosticoAsync(DiagnosticoDTO item);

        Task<List<PRMS>> ListarPRMS();

        Task EditPRMSAsync(int PacienteId, int PrmsId, string user_name);

        Task DeleteDiagnosticoAsync(int DiagnosticoId, string user_name);

        Task<PacienteDTO> GetDatosBasicosByIdAsync(int Id);

        Task<List<ContactoPacienteDTO>> GetContactosPacienteAsync(int PacienteId);

        Task<List<DiagnosticoDTO>> GetDiagnosticosAsync(int PacienteId);
    }
}
