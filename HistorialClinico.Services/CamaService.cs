using HistorialClinico.Domain;
using HistorialClinico.Infrastructure;
using HistorialClinico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HistorialClinico.Services
{
    public class CamaService : SqlHelperService, ICamaService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public CamaService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
        }

        public async Task MovimientoCamaPacienteAsync(string TipoMovimientoId, int CamaId, int PacienteId, string user_name)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CamaId", CamaId),
                new SqlParameter("PacienteId", PacienteId),
                new SqlParameter("TipoMovimientoId", TipoMovimientoId),
                new SqlParameter("UserName", user_name)
            };

            await ExecuteNonQueryAsync("sp_MovimientoCamaPaciente", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task<List<CamaPaciente>> GetCamasPacientesAsync()
        {
            var camas = await ExecuteReaderToListAsync<CamaPaciente>("sp_GetCamasPacientes", _connectionString, CommandType.StoredProcedure);

            return camas;
        }
    }
}