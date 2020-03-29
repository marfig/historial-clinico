using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using HistorialClinico.Infrastructure;
using HistorialClinico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace HistorialClinico.Services
{
    public class CirugiaService : SqlHelperService, ICirugiaService
    {
        #region Inicio
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;
        private readonly CultureInfo ci;

        public CirugiaService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            ci = CultureInfo.GetCultureInfo("es-ES");
        }
        #endregion

        #region Metodos publicos

        public async Task AddCirugiaAsync(CirugiaDTO model)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("PacienteId", model.PacienteId),
                new SqlParameter("TuvoCirugia", model.TuvoCirugia),
                new SqlParameter("Tecnica", model.Tecnica),
                new SqlParameter("Hallazgos", model.Hallazgos),
                new SqlParameter("Procedimiento", model.Procedimiento),
                new SqlParameter("CualProcedimiento", model.CualProcedimiento),
                new SqlParameter("OtrasAcotaciones", model.OtrasAcotaciones),
                new SqlParameter("UserName", model.UserAdd)
            };

            await ExecuteNonQueryAsync("sp_AddCirugia", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task<List<Cirugia>> ListCirugiaAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var items = await ExecuteReaderToListAsync<Cirugia>("sp_ListCirugia", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }
     
        #endregion
    }
}