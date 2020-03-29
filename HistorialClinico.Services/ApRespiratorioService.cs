using HistorialClinico.Domain.DTO;
using HistorialClinico.Domain.Entidades;
using HistorialClinico.Infrastructure;
using HistorialClinico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HistorialClinico.Services
{
    public class ApRespiratorioService : SqlHelperService, IApRespiratorioService
    {
        #region Inicio
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public ApRespiratorioService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
        }
        #endregion

        #region Métodos públicos

        public async Task<List<ApRespiratorio>> ListApRespiratorioAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var items = await ExecuteReaderToListAsync<ApRespiratorio>("sp_ListApRespiratorio", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<SoporteRespiratorio>> ListSoporteRespiratorioAsync()
        {
            var items = await ExecuteReaderToListAsync<SoporteRespiratorio>("select Id, Nombre, Parametros from SoporteRespiratorio", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<SoporteRespParametros>> ListSoporteRespParametrosAsync(int ApRespiratorioId)
        {
            SqlParameter param = new SqlParameter("ApRespiratorioId", ApRespiratorioId);

            var items = await ExecuteReaderToListAsync<SoporteRespParametros>("sp_ApRespiratorioParametros", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }
        
        public async Task<List<Ventilacion>> ListVentilacionAsync()
        {
            var items = await ExecuteReaderToListAsync<Ventilacion>("select Id, Nombre from Ventilacion", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<Modalidad>> ListModalidadAsync()
        {
            var items = await ExecuteReaderToListAsync<Modalidad>("select Id, Nombre from Modalidad", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<Gasometria>> ListGasometriaAsync()
        {
            var items = await ExecuteReaderToListAsync<Gasometria>("select Id, Nombre from Gasometria", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<GasometriaParametros>> ListGasometriaParametrosAsync(int ApRespiratorioId)
        {
            SqlParameter param = new SqlParameter("ApRespiratorioId", ApRespiratorioId);

            var items = await ExecuteReaderToListAsync<GasometriaParametros>("sp_ApRespiratorioGasometria", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task AddApRespiratorioAsync(ApRespiratorioDTO model)
        {
            var param_soporte = new List<ParametroDTO>();
            var param_gasom = new List<ParametroDTO>();

            if (!string.IsNullOrWhiteSpace(model.SoporteRespiratorioParamJSON))
            {
                param_soporte = JsonConvert.DeserializeObject<List<ParametroDTO>>(model.SoporteRespiratorioParamJSON);
            }

            if (!string.IsNullOrWhiteSpace(model.GasometriaParamJSON))
            {
                param_gasom = JsonConvert.DeserializeObject<List<ParametroDTO>>(model.GasometriaParamJSON);
            }

            var param_soporte_dt = ParametroToDataTable(param_soporte);
            var param_gasom_dt = ParametroToDataTable(param_gasom);

            var pParam = new SqlParameter("@ParamSoporteResp", SqlDbType.Structured)
            {
                TypeName = "dbo.ParametroList",
                Value = param_soporte_dt
            };

            var pParamGasom = new SqlParameter("@ParamGasometria", SqlDbType.Structured)
            {
                TypeName = "dbo.ParametroList",
                Value = param_gasom_dt
            };

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("PacienteId", model.PacienteId),
                new SqlParameter("Manejo", model.Manejo),
                new SqlParameter("Eventos", (model.Eventos ?? "")),
                new SqlParameter("Planes", (model.Planes ?? "")),
                pParam,
                pParamGasom,
                new SqlParameter("UserName", model.UserName)
            };

            if (model.SoporteRespiratorioId.HasValue)
            {
                parametros.Add(new SqlParameter("SoporteRespiratorioId", model.SoporteRespiratorioId.Value));
            }

            if (!string.IsNullOrWhiteSpace(model.ValorSoporteResp))
            {
                parametros.Add(new SqlParameter("ValorSoporteResp", model.ValorSoporteResp));
            }

            if (model.VentilacionId.HasValue)
            {
                parametros.Add(new SqlParameter("VentilacionId", model.VentilacionId.Value));
            }

            if (model.ModalidadId.HasValue)
            {
                parametros.Add(new SqlParameter("ModalidadId", model.ModalidadId.Value));
            }

            if (model.GasometriaId.HasValue)
            {
                parametros.Add(new SqlParameter("GasometriaId", model.GasometriaId.Value));
            }

            await ExecuteNonQueryAsync("sp_AddApRespiratorio", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        #endregion

        #region Helpers

        private DataTable ParametroToDataTable(List<ParametroDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("Valor", typeof(decimal)));

            foreach (var item in items)
            {
                dt.Rows.Add(item.Id, decimal.Parse(item.Valor));
            }

            return dt;
        }

        #endregion
    }
}