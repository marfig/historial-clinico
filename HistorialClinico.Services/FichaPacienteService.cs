using AutoMapper;
using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using HistorialClinico.Infrastructure;
using HistorialClinico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HistorialClinico.Services
{
    public class FichaPacienteService: SqlHelperService, IFichaPacienteService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public FichaPacienteService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
        }

        public async Task<IEnumerable<PacienteDTO>> ListarPacientesAsync(string Valor)
        {
            SqlParameter param = new SqlParameter("Valor", Valor);

            var item = await ExecuteReaderToListAsync<Paciente>("sp_ListPacientes", _connectionString, CommandType.StoredProcedure, param);

            return Mapper.Map<IEnumerable<PacienteDTO>>(item);
        }

        public async Task<PacienteDTO> AddEditDatosBasicosPacienteAsync(PacienteDTO item)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("UserName", item.UserName),
                new SqlParameter("Nombres", item.Nombres),
                new SqlParameter("Apellidos", item.Apellidos),
                new SqlParameter("Sexo", item.Sexo)
            };

            if (!string.IsNullOrWhiteSpace(item.NroDocumento))
            {
                parametros.Add(new SqlParameter("NroDocumento", item.NroDocumento));
            }
            else
            {
                parametros.Add(new SqlParameter("NroDocumento", DBNull.Value));
            }

            if (item.FechaNacimiento.HasValue)
            {
                parametros.Add(new SqlParameter("FechaNacimiento", item.FechaNacimiento.Value));
            }
            else
            {
                parametros.Add(new SqlParameter("FechaNacimiento", DBNull.Value));
            }

            if (!string.IsNullOrWhiteSpace(item.GrupoSanguineo))
            {
                parametros.Add(new SqlParameter("GrupoSanguineo", item.GrupoSanguineo));
            }
            else
            {
                parametros.Add(new SqlParameter("GrupoSanguineo", DBNull.Value));
            }

            if (item.Peso.HasValue)
            {
                parametros.Add(new SqlParameter("Peso", item.Peso.Value));
            }
            else
            {
                parametros.Add(new SqlParameter("Peso", DBNull.Value));
            }

            if (item.Id.HasValue)
            {
                parametros.Add(new SqlParameter("Id", item.Id.Value));
            }
            else
            {
                parametros.Add(new SqlParameter("Id", DBNull.Value));
            }

            if (item.CamaId.HasValue)
            {
                parametros.Add(new SqlParameter("CamaId", item.CamaId.Value));
            }
            else
            {
                parametros.Add(new SqlParameter("CamaId", DBNull.Value));
            }

            var paciente = await ExecuteReaderToSingleObjectAsync<Paciente>("sp_AddEditPaciente", _connectionString, CommandType.StoredProcedure, parametros.ToArray());

            return Mapper.Map<PacienteDTO>(paciente);
        }

        public async Task AddEditContactoPacienteAsync(IEnumerable<ContactoPacienteDTO> contactos)
        {
            contactos = contactos.Where(c => c.Id > 0 || !string.IsNullOrWhiteSpace(c.TipoContactoId) || !string.IsNullOrWhiteSpace(c.NombreContacto) || !string.IsNullOrWhiteSpace(c.NroContacto));

            foreach (var contacto in contactos)
            {
                if (contacto.Id > 0 && string.IsNullOrWhiteSpace(contacto.TipoContactoId) && string.IsNullOrWhiteSpace(contacto.NombreContacto) && string.IsNullOrWhiteSpace(contacto.NroContacto))
                {
                    await DeleteContactoPacienteAsync(contacto.Id, contacto.UserName);
                    continue;
                }

                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    new SqlParameter("Id", contacto.Id)
                };

                if (!string.IsNullOrWhiteSpace(contacto.TipoContactoId))
                {
                    parametros.Add(new SqlParameter("NombreContacto", contacto.NombreContacto));
                }
                else
                {
                    parametros.Add(new SqlParameter("NombreContacto", DBNull.Value));
                }

                if (!string.IsNullOrWhiteSpace(contacto.NroContacto))
                {
                    parametros.Add(new SqlParameter("NroContacto", contacto.NroContacto));
                }
                else
                {
                    parametros.Add(new SqlParameter("NroContacto", DBNull.Value));
                }

                if (!string.IsNullOrWhiteSpace(contacto.TipoContactoId))
                {
                    parametros.Add(new SqlParameter("TipoContacto", contacto.TipoContactoId));
                }
                else
                {
                    parametros.Add(new SqlParameter("TipoContacto", DBNull.Value));
                }

                parametros.Add(new SqlParameter("PacienteId", contacto.PacienteId));
                parametros.Add(new SqlParameter("UserName", contacto.UserName));

                await ExecuteNonQueryAsync("sp_AddEditContacto", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
            }
        }

        public async Task DeleteContactoPacienteAsync(int ContactoId, string user_name)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", ContactoId),
                new SqlParameter("UserName", user_name)
            };

            await ExecuteNonQueryAsync("sp_DeleteContacto", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task AddEditDiagnosticoAsync(DiagnosticoDTO item)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", item.Id),
                new SqlParameter("PacienteId", item.PacienteId),
                new SqlParameter("UserName", item.UserName)
            };

            if (!string.IsNullOrWhiteSpace(item.Resumen))
            {
                parametros.Add(new SqlParameter("Resumen", item.Resumen));
            }
            else
            {
                parametros.Add(new SqlParameter("Resumen", DBNull.Value));
            }

            await ExecuteNonQueryAsync("sp_AddEditDiagnostico", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task<List<PRMS>> ListarPRMS()
        {
            var items = await ExecuteReaderToListAsync<PRMS>("select Id, Nombre from PRMS", _connectionString, CommandType.Text);

            return items;
        }

        public async Task EditPRMSAsync(int PacienteId, int PrmsId, string user_name)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("PacienteId", PacienteId),
                new SqlParameter("UserName", user_name)
            };

            if (PrmsId > 0)
            {
                parametros.Add(new SqlParameter("PrmsId", PrmsId));
            }
            else
            {
                parametros.Add(new SqlParameter("PrmsId", DBNull.Value));
            }

            await ExecuteNonQueryAsync("sp_EditPRMS", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task DeleteDiagnosticoAsync(int DiagnosticoId, string user_name)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", DiagnosticoId),
                new SqlParameter("UserName", user_name)
            };

            await ExecuteNonQueryAsync("sp_DeleteDiagnostico", _connectionString, CommandType.StoredProcedure, parametros.ToArray());
        }

        public async Task<PacienteDTO> GetDatosBasicosByIdAsync(int Id)
        {
            SqlParameter param = new SqlParameter("Id", Id);

            var item = await ExecuteReaderToSingleObjectAsync<Paciente>("sp_GetPacienteById", _connectionString, CommandType.StoredProcedure, param);

            return Mapper.Map<PacienteDTO>(item);
        }

        public async Task<List<ContactoPacienteDTO>> GetContactosPacienteAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var contactos = await ExecuteReaderToListAsync<ContactoPaciente>("sp_GetContactosPaciente", _connectionString, CommandType.StoredProcedure, param);

            return Mapper.Map<List<ContactoPacienteDTO>>(contactos);
        }

        public async Task<List<DiagnosticoDTO>> GetDiagnosticosAsync(int PacienteId)
        {
            SqlParameter param = new SqlParameter("PacienteId", PacienteId);

            var items = await ExecuteReaderToListAsync<Diagnostico>("sp_GetDiagnosticos", _connectionString, CommandType.StoredProcedure, param);

            return Mapper.Map<List<DiagnosticoDTO>>(items);
        }
    }
}