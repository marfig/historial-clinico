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
    public class SNCService : SqlHelperService, ISNCService
    {
        #region Inicio
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;
        private readonly CultureInfo ci;

        public SNCService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            ci = CultureInfo.GetCultureInfo("es-ES");
        }
        #endregion

        #region Metodos publicos
        public async Task<List<AspectoGeneralSNC>> ListAspectoGralAsync()
        {
            var items = await ExecuteReaderToListAsync<AspectoGeneralSNC>("select Id, Nombre from AspectoGeneralSNC", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<Sedacion>> ListSedacionAsync()
        {
            var items = await ExecuteReaderToListAsync<Sedacion>("select Id, Nombre, RequeridoValor from Sedacion", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<MedicamentoSedacion>> ListMedicamentoSedacionAsync()
        {
            var items = await ExecuteReaderToListAsync<MedicamentoSedacion>("select Id, Nombre from MedicamentoSedacion", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<LaboratorioSNC>> ListLaboratorioSNCAsync()
        {
            var items = await ExecuteReaderToListAsync<LaboratorioSNC>("select Id, Nombre from LaboratorioSNC", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<ImagenSNC>> ListImagenSNCAsync()
        {
            var items = await ExecuteReaderToListAsync<ImagenSNC>("select Id, Nombre from ImagenSNC", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<SxAbstinenciaSNC>> ListSxAbstinenciaSNCAsync()
        {
            var items = await ExecuteReaderToListAsync<SxAbstinenciaSNC>("select Id, Nombre from SxAbstinenciaSNC", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<MedicacionSNC>> ListMedicacionSNCAsync()
        {
            var items = await ExecuteReaderToListAsync<MedicacionSNC>("select Id, Nombre from MedicacionSNC", _connectionString, CommandType.Text);

            return items;
        }

        public async Task<List<Anticonvulsionante>> ListAnticonvulsionanteAsync()
        {
            var items = await ExecuteReaderToListAsync<Anticonvulsionante>("select Id, Nombre from Anticonvulsionante", _connectionString, CommandType.Text);

            return items;
        }

        public async Task AddSNCAsync(SncDTO model)
        {
            var asp_gral = new List<SNSListasDTO>();
            var sedacion = new List<SNSListasDTO>();
            var lab = new List<SNSListasDTO>();
            var img = new List<SNSListasDTO>();
            var sx_abs = new List<SNSListasDTO>();
            var sx_abs_medicacion = new List<SNSListasDTO>();
            var conv = new List<SNSListasDTO>();

            if (!string.IsNullOrWhiteSpace(model.AspectoGralJSON))
            {
                asp_gral = JsonConvert.DeserializeObject<List<SNSListasDTO>>(model.AspectoGralJSON);
            }

            var asp_gral_dt = ListToDataTable(asp_gral);

            var pAspGral = new SqlParameter("@AspectoGral", SqlDbType.Structured)
            {
                TypeName = "dbo.SNCList",
                Value = asp_gral_dt
            };

            if (!string.IsNullOrWhiteSpace(model.SedacionMedicamentoJSON))
            {
                sedacion = JsonConvert.DeserializeObject<List<SNSListasDTO>>(model.SedacionMedicamentoJSON);
            }

            var sedacion_dt = ListToDataTable(sedacion);

            var pSedacion = new SqlParameter("@Sedacion", SqlDbType.Structured)
            {
                TypeName = "dbo.SNCList",
                Value = sedacion_dt
            };

            if (!string.IsNullOrWhiteSpace(model.LaboratorioJSON))
            {
                lab = JsonConvert.DeserializeObject<List<SNSListasDTO>>(model.LaboratorioJSON);
            }

            var lab_dt = ListToDataTable(lab);

            var pLab = new SqlParameter("@Laboratorio", SqlDbType.Structured)
            {
                TypeName = "dbo.SNCList",
                Value = lab_dt
            };

            if (!string.IsNullOrWhiteSpace(model.ImagenesJSON))
            {
                img = JsonConvert.DeserializeObject<List<SNSListasDTO>>(model.ImagenesJSON);
            }

            var img_dt = ListToDataTable(img);

            var pImg = new SqlParameter("@Imagenes", SqlDbType.Structured)
            {
                TypeName = "dbo.SNCList",
                Value = img_dt
            };

            if (!string.IsNullOrWhiteSpace(model.SxAbstinenciaJSON))
            {
                sx_abs = JsonConvert.DeserializeObject<List<SNSListasDTO>>(model.SxAbstinenciaJSON);
            }

            var sx_abs_dt = ListToDataTable(sx_abs);

            var pSxAbs = new SqlParameter("@SxAbstinencia", SqlDbType.Structured)
            {
                TypeName = "dbo.SNCList",
                Value = sx_abs_dt
            };

            if (!string.IsNullOrWhiteSpace(model.SxAbstinenciaMedicacionJSON))
            {
                sx_abs_medicacion = JsonConvert.DeserializeObject<List<SNSListasDTO>>(model.SxAbstinenciaMedicacionJSON);
            }

            var sx_abs_medicacion_dt = ListToDataTable(sx_abs_medicacion);

            var pSxAbsMed = new SqlParameter("@SxAbstinenciaMedicacion", SqlDbType.Structured)
            {
                TypeName = "dbo.SNCList",
                Value = sx_abs_medicacion_dt
            };

            if (!string.IsNullOrWhiteSpace(model.ConocidoConvulsionadorJSON))
            {
                conv = JsonConvert.DeserializeObject<List<SNSListasDTO>>(model.ConocidoConvulsionadorJSON);
            }

            var conv_dt = ListToDataTable(conv);

            var pConv = new SqlParameter("@Convulsionador", SqlDbType.Structured)
            {
                TypeName = "dbo.SNCList",
                Value = conv_dt
            };

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("PacienteId", model.PacienteId),
                pAspGral,
                new SqlParameter("SedacionId", model.SedacionId),
                pSedacion,
                new SqlParameter("ValorSedacion", model.ValorSedacion),
                pLab,
                pImg,
                new SqlParameter("SxAbstinenciaId", model.SxAbstinenciaId),
                pSxAbs,
                pSxAbsMed,
                new SqlParameter("ConocidoConvulsionadorId", model.ConocidoConvulsionadorId),
                pConv,
                new SqlParameter("Eventos", (model.Eventos ?? "")),
                new SqlParameter("Planes", (model.Planes ?? "")),
                new SqlParameter("UserName", model.UserName)
            };

            await ExecuteNonQueryAsync("sp_AddSNC", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task<List<SNC>> ListSNCAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var items = await ExecuteReaderToListAsync<SNC>("sp_ListSNC", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListSNC>> SNCAspectoGralAsync(int SNCId)
        {
            SqlParameter param = new SqlParameter("SNCId", SNCId);

            var items = await ExecuteReaderToListAsync<ListSNC>("sp_SNCAspectoGral", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }
               
        public async Task<List<ListSNC>> SNCMedicamentoSedacionAsync(int SNCId)
        {
            SqlParameter param = new SqlParameter("SNCId", SNCId);

            var items = await ExecuteReaderToListAsync<ListSNC>("sp_SNCMedicamentoSedacion", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListSNC>> SNCLaboratorioAsync(int SNCId)
        {
            SqlParameter param = new SqlParameter("SNCId", SNCId);

            var items = await ExecuteReaderToListAsync<ListSNC>("sp_SNCLaboratorio", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListSNC>> SNCImagenesAsync(int SNCId)
        {
            SqlParameter param = new SqlParameter("SNCId", SNCId);

            var items = await ExecuteReaderToListAsync<ListSNC>("sp_SNCImagenes", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListSNC>> SNCSxAbstinenciaAsync(int SNCId)
        {
            SqlParameter param = new SqlParameter("SNCId", SNCId);

            var items = await ExecuteReaderToListAsync<ListSNC>("sp_SNCSxAbstinencia", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListSNC>> SNCAbstinenciaMedicacionAsync(int SNCId)
        {
            SqlParameter param = new SqlParameter("SNCId", SNCId);

            var items = await ExecuteReaderToListAsync<ListSNC>("sp_SNCMedicacion", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }

        public async Task<List<ListSNC>> SNCAnticonvulsionanteAsync(int SNCId)
        {
            SqlParameter param = new SqlParameter("SNCId", SNCId);

            var items = await ExecuteReaderToListAsync<ListSNC>("sp_SNCAnticonvulsionante", _connectionString, CommandType.StoredProcedure, param);

            return items;
        }
        #endregion

        #region Helpers
        private DataTable ListToDataTable(List<SNSListasDTO> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("Fecha", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Valor", typeof(decimal)));

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Fecha))
                {
                    if (string.IsNullOrWhiteSpace(item.Valor))
                    {
                        dt.Rows.Add(item.Id);
                    }
                    else
                    {
                        dt.Rows.Add(item.Id, null, decimal.Parse(item.Valor, ci));
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(item.Valor))
                    {
                        dt.Rows.Add(item.Id, DateTime.Parse(item.Fecha, ci));
                    }
                
                    else
                    {
                        dt.Rows.Add(item.Id, DateTime.Parse(item.Fecha, ci), decimal.Parse(item.Valor, ci));
                    }
                }
                
            }

            return dt;
        }
        #endregion
    }
}