using HistorialClinico.Domain.DTO;
using HistorialClinico.Domain.Entidades;
using HistorialClinico.Infrastructure;
using HistorialClinico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace HistorialClinico.Services
{
    public class HMNService : SqlHelperService, IHMNService
    {
        #region Inicio
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;
        private readonly CultureInfo ci;

        public HMNService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            ci = CultureInfo.GetCultureInfo("es-ES");
        }
        #endregion

        #region Metodos publicos
        public async Task<List<GeneralHMN>> ListGeneralHMNAsync()
        {
            var items = await ExecuteReaderToListAsync<GeneralHMN>("select Id, Nombre, Valor, Formulacion from GeneralHMN", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<BalanceHidricoHMN>> ListBalanceHidricoHMNAsync()
        {
            var items = await ExecuteReaderToListAsync<BalanceHidricoHMN>("select Id, Nombre from BalanceHidricoHMN", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<LaboratorioHMN>> ListLaboratorioHMNAsync()
        {
            var items = 
                await ExecuteReaderToListAsync<LaboratorioHMN>("select l.Id, l.Nombre, c.Nombre as Categoria from LaboratorioHMN l inner join CategoriaLaboratorio c on l.CategoriaId = c.Id", 
                _connectionString, CommandType.Text);

            return items;
        }

        public async Task AddHMNAsync(HmnDTO model)
        {
            var gral = new List<HMNListasDTO>();
            var balance = new List<HMNListasDTO>();
            var lab = new List<HMNListasDTO>();

            if (!string.IsNullOrWhiteSpace(model.GeneralJSON))
            {
                gral = JsonConvert.DeserializeObject<List<HMNListasDTO>>(model.GeneralJSON);
            }

            var gral_dt = ListToDataTable(gral);

            var pGral = new SqlParameter("@General", SqlDbType.Structured)
            {
                TypeName = "dbo.HMNList",
                Value = gral_dt
            };

            if (!string.IsNullOrWhiteSpace(model.BalanceHidricoJSON))
            {
                balance = JsonConvert.DeserializeObject<List<HMNListasDTO>>(model.BalanceHidricoJSON);
            }

            var balance_dt = ListToDataTable(balance);

            var pBalance = new SqlParameter("@BalanceHidrico", SqlDbType.Structured)
            {
                TypeName = "dbo.HMNList",
                Value = balance_dt
            };

            if (!string.IsNullOrWhiteSpace(model.LaboratorioJSON))
            {
                lab = JsonConvert.DeserializeObject<List<HMNListasDTO>>(model.LaboratorioJSON);
            }

            var lab_dt = ListToDataTable(lab);

            var pLab = new SqlParameter("@Laboratorio", SqlDbType.Structured)
            {
                TypeName = "dbo.HMNList",
                Value = lab_dt
            };

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("PacienteId", model.PacienteId),
                new SqlParameter("DialisisPeritoneal", model.DialisisPeritoneal),
                new SqlParameter("FormulacionDialisisPeritoneal", (model.FormulacionDialisisPeritoneal ?? "")),
                pGral,
                pBalance,
                pLab,
                new SqlParameter("Eventos", (model.Eventos ?? "")),
                new SqlParameter("Planes", (model.Planes ?? "")),
                new SqlParameter("UserName", model.UserName)
            };

            await ExecuteNonQueryAsync("sp_AddHMN", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task<List<HMN>> ListHMNAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var items = await ExecuteReaderToListAsync<HMN>("sp_ListHMN", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListHMN>> HMNGeneralAsync(int HMNId)
        {
            SqlParameter param = new SqlParameter("@HMNId", HMNId);

            var items = await ExecuteReaderToListAsync<ListHMN>("sp_HMNGeneral", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListHMN>> HMNBalanceHidricoHMNlAsync(int HMNId)
        {
            SqlParameter param = new SqlParameter("@HMNId", HMNId);

            var items = await ExecuteReaderToListAsync<ListHMN>("sp_HMNBalanceHidricoHMN", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListHMN>> HMNLaboratorioHMNAsync(int HMNId)
        {
            SqlParameter param = new SqlParameter("@HMNId", HMNId);

            var items = await ExecuteReaderToListAsync<ListHMN>("sp_HMNLaboratorioHMN", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        #endregion
        
        #region Helpers
        private DataTable ListToDataTable(List<HMNListasDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("Valor", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Formulacion", typeof(string)));

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Valor))
                {
                    dt.Rows.Add(item.Id, null, item.Formulacion);
                }
                else
                {
                    dt.Rows.Add(item.Id, item.Valor);
                }
            }

            return dt;
        }
        #endregion
    
    }
}