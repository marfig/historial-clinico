#region Using

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HistorialClinico.Web.Models.Paciente;
using System.Collections.Generic;
using HistorialClinico.Web.Models.Kendo;
using HistorialClinico.Services.Interfaces;
using System;
using System.Globalization;
using System.Threading.Tasks;
using HistorialClinico.Domain;
using HistorialClinico.Domain.DTO;
using Newtonsoft.Json;
using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion

namespace HistorialClinico.Web.Controllers
{
    [Authorize]
    public class PacienteController : Controller
    {
        #region Inicio
        private readonly IFichaPacienteService _fichaPacienteService;
        private readonly ICamaService _camaService;

        public PacienteController(IFichaPacienteService fichaPacienteService,
                                  ICamaService camaService)
        {
            _fichaPacienteService = fichaPacienteService;
            _camaService = camaService;
        }

        #endregion

        #region Actions
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Listado(DataSourceRequest command)
        {
            List<PacienteGridModel> model = PacientesList();
           
            var gridModel = new DataSourceResult
            {
                Data = model,
                Total = model.Count
            };
            return Json(gridModel);
        }

        public async Task<ActionResult> Add(int Id = 0, int CamaId = 0)
        {
            await CargarListados();

            PacienteFormModel model = new PacienteFormModel()
            {
                Id = Id,
                CamaId = CamaId,
                Contactos = new List<ContactoPacienteFormModel>()
                {
                    new ContactoPacienteFormModel()
                }
            };

            if (Id > 0)
            {
                var ci = CultureInfo.GetCultureInfo("es-ES");

                var item = await _fichaPacienteService.GetDatosBasicosByIdAsync(Id);
                model.Id = item.Id.Value;
                model.Nombre = item.Nombres;
                model.Apellido = item.Apellidos;
                model.Sexo = item.Sexo;
                model.NroDocumento = item.NroDocumento;
                model.GrupoSanguineo = item.GrupoSanguineo;
                model.PrmsId = item.PrmsId;

                if (item.FechaNacimiento.HasValue)
                {
                    model.FechaNacimiento = item.FechaNacimiento.Value.ToString("d", ci);
                }

                if (item.Peso.HasValue)
                {
                    model.Peso = item.Peso.Value.ToString("N3", ci);
                }

                var contactos = await _fichaPacienteService.GetContactosPacienteAsync(Id);

                if (contactos.Count() > 0)
                {
                    model.Contactos = contactos.Select(c => new ContactoPacienteFormModel()
                    {
                        ContactoId = c.Id,
                        TipoContactoId = c.TipoContactoId,
                        NombreContacto = c.NombreContacto,
                        NroContacto = c.NroContacto
                    }).ToList();
                }
            }

            return View(model);
        }
        
        public PartialViewResult AddContactoPaciente()
        {
            List<ContactoPacienteFormModel> model = new List<ContactoPacienteFormModel>()
            {
                new ContactoPacienteFormModel()
            };

            return PartialView("_ContactoPaciente", model);
        }

        public async Task<PartialViewResult> GetContactosPaciente(int PacienteId)
        {
            var contactos = await _fichaPacienteService.GetContactosPacienteAsync(PacienteId);

            var model = contactos.Select(c => new ContactoPacienteFormModel()
            {
                ContactoId = c.Id,
                TipoContactoId = c.TipoContactoId,
                NombreContacto = c.NombreContacto,
                NroContacto = c.NroContacto
            }).ToList();

            return PartialView("_ContactoPaciente", model);
        }

        [HttpPost]
        public async Task<JsonResult> AddDatosBasicos(PacienteFormModel model)
        {
            var ci = CultureInfo.GetCultureInfo("es-ES");

            PacienteDTO item = new PacienteDTO()
            {
                NroDocumento = model.NroDocumento,
                Nombres = model.Nombre,
                Apellidos = model.Apellido,
                Sexo = model.Sexo,
                GrupoSanguineo = model.GrupoSanguineo,
                UserName = User.Identity.Name
            };

            if (model.Id > 0)
            {
                item.Id = model.Id;
            }

            if (model.CamaId > 0)
            {
                item.CamaId = model.CamaId;
            }

            if (!string.IsNullOrWhiteSpace(model.FechaNacimiento))
            {
                item.FechaNacimiento = DateTime.Parse(model.FechaNacimiento, ci);
            }

            if (!string.IsNullOrWhiteSpace(model.Peso))
            {
                item.Peso = decimal.Parse(model.Peso.Replace(".", ""));
            }

            var paciente = await _fichaPacienteService.AddEditDatosBasicosPacienteAsync(item);

            var contactos = JsonConvert.DeserializeObject<List<ContactoPacienteFormModel>>(model.ContactosJSON);

            var contactos_dto = contactos.Select(c => new ContactoPacienteDTO()
            {
                Id = c.ContactoId,
                TipoContactoId = c.TipoContactoId,
                NombreContacto = c.NombreContacto,
                NroContacto = c.NroContacto,
                PacienteId = paciente.Id.Value,
                UserName = User.Identity.Name
            });

            await _fichaPacienteService.AddEditContactoPacienteAsync(contactos_dto);

            return Json(new { Success = true, PacienteId = paciente.Id });
        }

        public async Task<JsonResult> AddDiagnostico(DiagnosticoFormModel model)
        {
            DiagnosticoDTO item = new DiagnosticoDTO()
            {
                Id = model.Id,
                PacienteId = model.PacienteId,
                Resumen = model.Resumen,
                UserName = User.Identity.Name
            };

            await _fichaPacienteService.AddEditDiagnosticoAsync(item);

            return Json(new { Success = true });
        }

        public async Task<ActionResult> ListDiagnosticos(int PacienteId)
        {
            var model = await _fichaPacienteService.GetDiagnosticosAsync(PacienteId);

            var gridModel = new DataSourceResult
            {
                Data = model
            };
            return Json(gridModel);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDiagnostico(int DiagnosticoId)
        {
            await _fichaPacienteService.DeleteDiagnosticoAsync(DiagnosticoId, User.Identity.Name);

            return Json(new { Success = true });
        }

        [HttpPost]
        public async Task<JsonResult> EditPRMS(int PacienteId, int? PrmsId = 0)
        {
            await _fichaPacienteService.EditPRMSAsync(PacienteId, (PrmsId ?? 0), User.Identity.Name);

            return Json(new { Success = true });
        }

        public async Task<JsonResult> BuscarPaciente(string Valor)
        {
            var pacientes = await _fichaPacienteService.ListarPacientesAsync(Valor);

            var result = from c in pacientes select new { c.Id, c.Nombres, c.Apellidos, NroDocumento = (c.NroDocumento ?? "") };
            return Json(result);
        }

        #endregion

        #region Helpers
        private List<PacienteGridModel> PacientesList()
        {
            List<PacienteGridModel> model = new List<PacienteGridModel>()
            {
                new PacienteGridModel()
                {
                    Id = 1,
                    Cama = "1",
                    Nombre = "Lucía Ugarte",
                    Sexo = "Femenino",
                    Edad = "5 años"
                },
                new PacienteGridModel()
                {
                    Id = 2,
                    Cama = "2",
                    Nombre = "Jorge Ortega",
                    Sexo = "Masculino",
                    Edad = "8 años"
                },
                new PacienteGridModel()
                {
                    Id = 3,
                    Cama = "3",
                    Nombre = "Oscar Naruto",
                    Sexo = "Masculino",
                    Edad = "7 meses"
                },
                new PacienteGridModel()
                {
                    Id = 4,
                    Cama = "4",
                    Nombre = "Karina Ramírez",
                    Sexo = "Femenino",
                    Edad = "3 años"
                },
                new PacienteGridModel()
                {
                    Id = 5,
                    Cama = "5",
                    Nombre = "Héctor Aquino",
                    Sexo = "Masculino",
                    Edad = "1 año, 3 meses"
                },
                new PacienteGridModel()
                {
                    Id = 6,
                    Cama = "6"
                },
                new PacienteGridModel()
                {
                    Id = 7,
                    Cama = "7"
                },
                new PacienteGridModel()
                {
                    Id = 8,
                    Cama = "8"
                },
                new PacienteGridModel()
                {
                    Id = 9,
                    Cama = "9"
                },
                new PacienteGridModel()
                {
                    Id = 10,
                    Cama = "10"
                },
            };

            return model;
        }

        private async Task CargarListados()
        {
            var prms = await _fichaPacienteService.ListarPRMS();
            ViewBag.PRMS = prms.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });
        }
        #endregion
    }
}
