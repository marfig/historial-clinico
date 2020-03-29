using HistorialClinico.Domain.DTO;
using HistorialClinico.Domain.Entidades;
using HistorialClinico.Infrastructure;
using HistorialClinico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace HistorialClinico.Services
{
    public class InfectologicoService : SqlHelperService, IInfectologicoService
    {
        #region Inicio
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;
        private readonly CultureInfo ci;

        public InfectologicoService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            ci = CultureInfo.GetCultureInfo("es-ES");
        }
        #endregion

        #region Métodos públicos

        public async Task<List<Infectologico>> ListInfectologicoAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var items = await ExecuteReaderToListAsync<Infectologico>("sp_ListInfectologico", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<InfectologicoCoberturaAtb>> InfectologicoCoberturaAtbAsync(int InfectologicoId)
        {
            SqlParameter param = new SqlParameter("InfectologicoId", InfectologicoId);

            var items = await ExecuteReaderToListAsync<InfectologicoCoberturaAtb>("sp_InfectologicoCoberturaAtb", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<InfectologicoCultivo>> InfectologicoCultivoAsync(int InfectologicoId)
        {
            SqlParameter param = new SqlParameter("InfectologicoId", InfectologicoId);

            var items = await ExecuteReaderToListAsync<InfectologicoCultivo>("sp_InfectologicoCultivo", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<SensibilidadCultivoInfectologico>> SensibilidadCultivoInfectologicoAsync(int InfectologicoId, int CultivoId)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("InfectologicoId", InfectologicoId),
                new SqlParameter("CultivoId", CultivoId)
            };

            var items = await ExecuteReaderToListAsync<SensibilidadCultivoInfectologico>("sp_SensibilidadCultivoInfectologico", _connectionString, CommandType.StoredProcedure, parametros.ToArray());

            return items;
        }

        public async Task<List<HisopadoInfectologico>> HisopadoInfectologicoAsync(int InfectologicoId)
        {
            SqlParameter param = new SqlParameter("InfectologicoId", InfectologicoId);

            var items = await ExecuteReaderToListAsync<HisopadoInfectologico>("sp_HisopadoInfectologico", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<EstadoInfectologico>> ListEstadoInfectologicoAsync()
        {
            var items = await ExecuteReaderToListAsync<EstadoInfectologico>("select Id, Nombre from EstadoInfectologico", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<Cultivo>> ListCultivoAsync()
        {
            var items = await ExecuteReaderToListAsync<Cultivo>("select Id, Nombre from Cultivo", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<Hisopado>> ListHisopadoAsync()
        {
            var items = await ExecuteReaderToListAsync<Hisopado>("select Id, Nombre from Hisopado", _connectionString, CommandType.Text);

            return items;
        }

        public async Task AddInfectologicoAsync(InfectologicoDTO model)
        {
            var param_atb = new List<CoberturaAtbDTO>();
            var param_cultivo = new List<CultivoDTO>();
            var param_sensibilidad = new List<SensibilidadDTO>();
            var param_hisopado = new List<HisopadoDTO>();

            if (!string.IsNullOrWhiteSpace(model.CoberturaAtbParamJSON))
            {
                param_atb = JsonConvert.DeserializeObject<List<CoberturaAtbDTO>>(model.CoberturaAtbParamJSON);
            }

            if (!string.IsNullOrWhiteSpace(model.CultivoParamJSON))
            {
                param_cultivo = JsonConvert.DeserializeObject<List<CultivoDTO>>(model.CultivoParamJSON);
            }

            if (!string.IsNullOrWhiteSpace(model.SensibilidadParamJSON))
            {
                param_sensibilidad = JsonConvert.DeserializeObject<List<SensibilidadDTO>>(model.SensibilidadParamJSON);
            }

            if (!string.IsNullOrWhiteSpace(model.HisopadoParamJSON))
            {
                param_hisopado = JsonConvert.DeserializeObject<List<HisopadoDTO>>(model.HisopadoParamJSON);
            }

            var param_atb_dt = CoberturaAtbToDataTable(param_atb);
            var param_cultivo_dt = CultivoToDataTable(param_cultivo);
            var param_sensibilidad_dt = SensibilidadToDataTable(param_sensibilidad);
            var param_hisopado_dt = HisopadoToDataTable(param_hisopado);

            var pAtb = new SqlParameter("@Antibioticos", SqlDbType.Structured)
            {
                TypeName = "dbo.AntibioticosList",
                Value = param_atb_dt
            };

            var pCultivo = new SqlParameter("@Cultivos", SqlDbType.Structured)
            {
                TypeName = "dbo.CultivosList",
                Value = param_cultivo_dt
            };

            var pSensibilidad = new SqlParameter("@Sensibilidad", SqlDbType.Structured)
            {
                TypeName = "dbo.SensibilidadList",
                Value = param_sensibilidad_dt
            };

            var pHisopado = new SqlParameter("@Hisopado", SqlDbType.Structured)
            {
                TypeName = "dbo.HisopadoList",
                Value = param_hisopado_dt
            };

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("PacienteId", model.PacienteId),
                new SqlParameter("EstadoInfectologicoId", model.EstadoInfectologicoId),
                pAtb,
                pCultivo,
                pSensibilidad,
                pHisopado,
                new SqlParameter("Interconsulta", (model.Interconsulta ?? "")),
                new SqlParameter("Eventos", (model.Eventos ?? "")),
                new SqlParameter("Planes", (model.Planes ?? "")),
                new SqlParameter("UserName", model.UserName)
            };

            await ExecuteNonQueryAsync("sp_AddInfectologico", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        #endregion

        #region Helpers

        private DataTable CoberturaAtbToDataTable(List<CoberturaAtbDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Antibiotico", typeof(string)));
            dt.Columns.Add(new DataColumn("Dosis", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Unidad", typeof(string)));
            dt.Columns.Add(new DataColumn("AjustadoClearence", typeof(bool)));
            dt.Columns.Add(new DataColumn("FechaInicio", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("FechaSuspension", typeof(DateTime)));

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.FechaSuspension))
                {
                    dt.Rows.Add(item.Antibiotico, decimal.Parse(item.Dosis, ci), item.Unidad, item.AjustadoClearence, DateTime.Parse(item.FechaInicio, ci));
                }
                else
                {
                    dt.Rows.Add(item.Antibiotico, decimal.Parse(item.Dosis, ci), item.Unidad, item.AjustadoClearence, DateTime.Parse(item.FechaInicio, ci), DateTime.Parse(item.FechaSuspension, ci));
                }
            }

            return dt;
        }

        private DataTable CultivoToDataTable(List<CultivoDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CultivoId", typeof(int)));
            dt.Columns.Add(new DataColumn("Resultado", typeof(bool)));
            dt.Columns.Add(new DataColumn("Fecha", typeof(DateTime)));

            foreach (var item in items)
            {
                dt.Rows.Add(item.CultivoId, item.Resultado, DateTime.Parse(item.Fecha, ci));
            }

            return dt;
        }

        private DataTable SensibilidadToDataTable(List<SensibilidadDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CultivoId", typeof(int)));
            dt.Columns.Add(new DataColumn("Sensibilidad", typeof(string)));

            foreach (var item in items)
            {
                dt.Rows.Add(item.CultivoId, item.Sensibilidad);
            }

            return dt;
        }

        private DataTable HisopadoToDataTable(List<HisopadoDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("HisopadoId", typeof(int)));
            dt.Columns.Add(new DataColumn("Resultado", typeof(bool)));
            dt.Columns.Add(new DataColumn("Fecha", typeof(DateTime)));

            foreach (var item in items)
            {
                dt.Rows.Add(item.HisopadoId, item.Resultado, DateTime.Parse(item.Fecha, ci));
            }

            return dt;
        }

        #endregion
    }
}