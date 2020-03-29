#region Using

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HistorialClinico.Web.Models.Paciente;
using System.Collections.Generic;
using HistorialClinico.Web.Models.Kendo;
using HistorialClinico.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System;

#endregion

namespace HistorialClinico.Web.Controllers
{
    [Authorize]
    public class CamaPacienteController : Controller
    {
        #region Inicio
        private readonly ICamaService _camaService;

        public CamaPacienteController(ICamaService camaService)
        {
            _camaService = camaService;
        }

        #endregion

        #region Actions

        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> _Listado()
        {
            var items = await _camaService.GetCamasPacientesAsync();

            var model = items.Select(c => new CamaPacienteGridModel()
            {
                Id = c.Id,
                Cama = c.Cama,
                PacienteId = c.PacienteId,
                NombrePaciente = !c.PacienteId.HasValue ? "" : c.NombrePaciente,
                DiasInternacion = !c.PacienteId.HasValue ? 0 : (DateTime.Now - c.FechaIngreso.Value).Days,
                Edad = Utils.FuncionesUtiles.CalcularEdad(c.FechaNacimiento)
            }).ToList();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> AsociarCamaPaciente(int PacienteId, int CamaId)
        {
            await _camaService.MovimientoCamaPacienteAsync("ING", CamaId, PacienteId, User.Identity.Name);

            return Json(new { Success = true });
        }
        
        [HttpPost]
        public async Task<JsonResult> DesasociarCamaPaciente(int PacienteId, int CamaId)
        {
            await _camaService.MovimientoCamaPacienteAsync("EGR", CamaId, PacienteId, User.Identity.Name);

            return Json(new { Success = true });
        }

        #endregion

    }
}
