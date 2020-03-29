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
    public class HematologicoService : SqlHelperService, IHematologicoService
    {
        #region Inicio
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;
        private readonly CultureInfo ci;

        public HematologicoService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            ci = CultureInfo.GetCultureInfo("es-ES");
        }
        #endregion

        #region Metodos publicos

        public async Task AddHematologicoAsync(HematologicoDTO model)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("PacienteId", model.PacienteId),
                new SqlParameter("Hemograma_PAI", model.Hemograma_PAI),
                new SqlParameter("Hemograma_HB", model.Hemograma_HB),
                new SqlParameter("Hemograma_HTC", model.Hemograma_HTC),
                new SqlParameter("Hemograma_PLT", model.Hemograma_PLT),
                new SqlParameter("Crasis_TP", model.Crasis_TP),
                new SqlParameter("Crasis_TTPA", model.Crasis_TTPA),
                new SqlParameter("Crasis_Fibrinoginos", model.Crasis_Fibrinoginos),
                new SqlParameter("VitaminaK", model.VitaminaK),
                new SqlParameter("DosisVitaminaK", model.DosisVitaminaK),
                new SqlParameter("FechaVitaminaK", model.FechaVitaminaK),
                new SqlParameter("SangradoActivo", model.SangradoActivo),
                new SqlParameter("LugarSangrado", model.LugarSangrado),
                new SqlParameter("Transfusiones", model.Transfusiones),
                new SqlParameter("Transfusiones_GRC", model.Transfusiones_GRC),
                new SqlParameter("Transfusiones_PFC", model.Transfusiones_PFC),
                new SqlParameter("Transfusiones_CRIO", model.Transfusiones_CRIO),
                new SqlParameter("Transfusiones_PLT", model.Transfusiones_PLT),
                new SqlParameter("Eventos", (model.Eventos ?? "")),
                new SqlParameter("Planes", (model.Planes ?? "")),
                new SqlParameter("UserName", model.UserName)
            };

            await ExecuteNonQueryAsync("sp_AddHematologico", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task<List<Hematologico>> ListHematologicoAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var items = await ExecuteReaderToListAsync<Hematologico>("sp_ListHematologico", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

     
        #endregion
    }
}