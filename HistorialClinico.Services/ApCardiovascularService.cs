using AutoMapper;
using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
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
    public class ApCardiovascularService : SqlHelperService, IApCardiovascularService
    {
        #region Inicio
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public ApCardiovascularService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
        }
        #endregion

        #region Métodos públicos

        public async Task<List<ApCardiovascularDTO>> ListApCardiovascularAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var items = await ExecuteReaderToListAsync<ApCardiovascular>("sp_ListApCardiovascular", _connectionString, CommandType.StoredProcedure, param);

            return Mapper.Map<List<ApCardiovascularDTO>>(items);
        }

        public async Task<List<Inotropico>> ListInotropicosAsync(int ApCardiovascularId, bool Relacionados)
        {
            SqlParameter p_ApCardiovascularId = new SqlParameter("ApCardiovascularId", ApCardiovascularId);
            SqlParameter p_Relacionados = new SqlParameter("Relacionados", Relacionados);

            var items = await ExecuteReaderToListAsync<Inotropico>("sp_ApCardiovascularInotropicos", _connectionString, CommandType.StoredProcedure, p_ApCardiovascularId, p_Relacionados);

            return items;
        }

        public async Task<List<EnzimaCardiaca>> ListEnzimasCardiacasAsync(int ApCardiovascularId)
        {
            SqlParameter p_ApCardiovascularId = new SqlParameter("ApCardiovascularId", ApCardiovascularId);

            var items = await ExecuteReaderToListAsync<EnzimaCardiaca>("sp_ApCardiovascularEnzimas", _connectionString, CommandType.StoredProcedure, p_ApCardiovascularId);

            return items;
        }

        public async Task AddApCardiovascularAsync(int PacienteId, string EstadoId, string EvaluacionCardiologica, string InotropicosJSON, string EnzimasCardiacasJSON, string Eventos, string Planes, string user_name)
        {
            var inotropicos = new List<InotropicoDTO>();
            var enzimas = new List<EnzimaCardiacaDTO>();

            if (!string.IsNullOrWhiteSpace(InotropicosJSON))
            {
                inotropicos = JsonConvert.DeserializeObject<List<InotropicoDTO>>(InotropicosJSON);
            }

            if (!string.IsNullOrWhiteSpace(EnzimasCardiacasJSON))
            {
                enzimas = JsonConvert.DeserializeObject<List<EnzimaCardiacaDTO>>(EnzimasCardiacasJSON);
            }

            var inotropicos_dt = InotropicosToDataTable(inotropicos);
            var enzimas_dt = EnzimasToDataTable(enzimas);

            var pInotropicos = new SqlParameter("@Inotropicos", SqlDbType.Structured)
            {
                TypeName = "dbo.InotropicoList",
                Value = inotropicos_dt
            };

            var pEnzimas = new SqlParameter("@Enzimas", SqlDbType.Structured)
            {
                TypeName = "dbo.EnzimaCardiacaList",
                Value = enzimas_dt
            };

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("PacienteId", PacienteId),
                new SqlParameter("EstadoId", EstadoId),
                new SqlParameter("Eventos", (Eventos ?? "")),
                new SqlParameter("Planes", (Planes ?? "")),
                new SqlParameter("EvaluacionCardiologica", EvaluacionCardiologica),
                pInotropicos,
                pEnzimas,
                new SqlParameter("UserName", user_name)
            };

            await ExecuteNonQueryAsync("sp_AddApCardiovascular", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        #endregion

        #region Helpers

        private DataTable InotropicosToDataTable(List<InotropicoDTO> items)
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

        private DataTable EnzimasToDataTable(List<EnzimaCardiacaDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("Valor", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Curva", typeof(string)));

            foreach (var item in items)
            {
                dt.Rows.Add(item.Id, decimal.Parse(item.Valor), item.Curva);
            }

            return dt;
        }

        #endregion
    }
}