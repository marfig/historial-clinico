#region Using

using AutoMapper;
using HistorialClinico.Domain.DTO;
using HistorialClinico.Services.Interfaces;
using HistorialClinico.Web.Models.Paciente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace HistorialClinico.Web.Controllers
{
    [Authorize]
    public class ResumenPacienteController : Controller
    {
        private readonly IFichaPacienteService _pacienteService;
        private readonly IApCardiovascularService _apCardioService;
        private readonly IApRespiratorioService _apRespService;
        private readonly IInfectologicoService _infectoService;
        private readonly ISNCService _sncService;
        private readonly IHMNService _hmnService;
        private readonly IHematologicoService _hematologicoService;
        private readonly IFichaPacienteService _fichaPacienteService;
        private readonly ICirugiaService _cirugiaService;

        public ResumenPacienteController(IFichaPacienteService pacienteService,
                                   IApCardiovascularService apCardioService,
                                   IApRespiratorioService apRespService,
                                   IInfectologicoService infectoService,
                                   ISNCService sncService,
                                   IHMNService hmnService,
                                   IHematologicoService hematologicoService,
                                   IFichaPacienteService fichaPacienteService,
                                   ICirugiaService cirugiaService)
        {
            _pacienteService = pacienteService;
            _apCardioService = apCardioService;
            _apRespService = apRespService;
            _infectoService = infectoService;
            _sncService = sncService;
            _hmnService = hmnService;
            _hematologicoService = hematologicoService;
            _fichaPacienteService = fichaPacienteService;
            _cirugiaService = cirugiaService;
        }

        public async Task<IActionResult> Index(int PacienteId)
        {
            ViewBag.PacienteId = PacienteId;

            var item = await _pacienteService.GetDatosBasicosByIdAsync(PacienteId);

            var model = new ResumenPacienteModel()
            {
                Nombre = $"{item.Nombres} {item.Apellidos}",
                FechaNacimiento = item.FechaNacimiento,
                Sexo = item.Sexo == "M" ? "Masculino" : item.Sexo == "F" ? "Femenino" : "",
                GrupoSanguineo = item.GrupoSanguineo,
                Edad = Utils.FuncionesUtiles.CalcularEdad(item.FechaNacimiento)
            };

            return View(model);

        }

        public async Task<PartialViewResult> _ApCardiovascular(int PacienteId)
        {
            var items = await _apCardioService.ListApCardiovascularAsync(PacienteId);

            List<ApCardiovascularFormModel> model = new List<ApCardiovascularFormModel>();

            foreach (var item in items)
            {
                var new_item = new ApCardiovascularFormModel()
                {
                    Id = item.Id,
                    Estado = EstadoApCardiovascular(item.EstadoId),
                    EvaluacionCardiologica = item.EvaluacionCardiologica,
                    Eventos = item.Eventos,
                    Planes = item.Planes,
                    UserName = item.UserAdd,
                    DateAdd = item.DateAdd
                };

                var inotropicos = await _apCardioService.ListInotropicosAsync(item.Id, true);
                new_item.Inotropicos = inotropicos.Select(c => new InotropicosModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor
                });

                var enzimas = await _apCardioService.ListEnzimasCardiacasAsync(item.Id);
                new_item.EnzimasCardiacas = enzimas.Select(c => new EnzimasCardiacasModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor,
                    Curva = CurvaEnzimas(c.Curva)
                });

                model.Add(new_item);
            }

            return PartialView(model);
        }

        public async Task<PartialViewResult> _ApRespiratorio(int PacienteId)
        {
            var items = await _apRespService.ListApRespiratorioAsync(PacienteId);

            List<ApRespiratorioFormModel> model = new List<ApRespiratorioFormModel>();

            foreach (var item in items)
            {
                var new_item = new ApRespiratorioFormModel()
                {
                    Id = item.Id,
                    SoporteRespiratorio = item.SoporteRespiratorio,
                    Parametros = item.Parametros,
                    ValorSoporteResp = item.ValorSoporteResp,
                    Ventilacion = item.Ventilacion,
                    Modalidad = item.Modalidad,
                    Gasometria = item.Gasometria,
                    Manejo = item.Manejo,
                    Eventos = item.Eventos,
                    Planes = item.Planes,
                    UserName = item.UserAdd,
                    DateAdd = item.DateAdd
                };

                var param_sop = await _apRespService.ListSoporteRespParametrosAsync(item.Id);
                new_item.SoporteRespiratorioParam = param_sop.Select(c => new ParametrosApRespModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor
                });

                var param_gas = await _apRespService.ListGasometriaParametrosAsync(item.Id);
                new_item.GasometriaParam = param_gas.Select(c => new ParametrosApRespModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor
                });

                model.Add(new_item);
            }

            return PartialView(model);
        }

        public async Task<PartialViewResult> _Cirugia(int PacienteId)
        {
            var items = await _cirugiaService.ListCirugiaAsync(PacienteId);

            var model = Mapper.Map<List<CirugiaDTO>>(items);

            return PartialView(model);
        }

        public async Task<PartialViewResult> _Hematologico(int PacienteId)
        {
            var paciente = await _fichaPacienteService.GetDatosBasicosByIdAsync(PacienteId);
            var items = await _hematologicoService.ListHematologicoAsync(PacienteId);

            ViewBag.GrupoSanguineo = paciente.GrupoSanguineo;

            var model = Mapper.Map<List<HematologicoFormModel>>(items);


            return PartialView(model);
        }

        public async Task<PartialViewResult> _HMN(int PacienteId)
        {
            var items = await _hmnService.ListHMNAsync(PacienteId);

            List<HMNFormModel> model = new List<HMNFormModel>();

            foreach (var item in items)
            {
                var new_item = new HMNFormModel()
                {
                    Id = item.Id,
                    DialisisPeritoneal = item.DialisisPeritoneal,
                    FormulacionDialisisPeritoneal = item.FormulacionDialisisPeritoneal,
                    Eventos = item.Eventos,
                    Planes = item.Planes,
                    UserName = item.UserAdd,
                    DateAdd = item.DateAdd
                };

                var gral = await _hmnService.HMNGeneralAsync(item.Id);
                new_item.General = gral.Select(c => new ListHMNModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor,
                    Formulacion = c.Formulacion
                });

                var balance_hid = await _hmnService.HMNBalanceHidricoHMNlAsync(item.Id);
                new_item.BalanceHidrico = balance_hid.Select(c => new ListHMNModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor
                });

                var lab = await _hmnService.HMNLaboratorioHMNAsync(item.Id);
                new_item.Laboratorio = lab.Select(c => new ListHMNModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor,
                    Categoria = c.Categoria
                });

                model.Add(new_item);
            }

            return PartialView(model);
        }

        public async Task<PartialViewResult> _Infecto(int PacienteId)
        {
            var items = await _infectoService.ListInfectologicoAsync(PacienteId);

            List<InfectologicoFormModel> model = new List<InfectologicoFormModel>();

            foreach (var item in items)
            {
                var new_item = new InfectologicoFormModel()
                {
                    Id = item.Id,
                    EstadoInfectologico = item.EstadoInfectologico,
                    Interconsulta = item.Interconsulta,
                    Eventos = item.Eventos,
                    Planes = item.Planes,
                    UserName = item.UserAdd,
                    DateAdd = item.DateAdd
                };

                var cobertura = await _infectoService.InfectologicoCoberturaAtbAsync(item.Id);
                new_item.CoberturaAtb = cobertura.Select(c => new CoberturaAtbModel()
                {
                    Antibiotico = c.Antibiotico,
                    Dosis = c.Dosis,
                    Unidad = c.Unidad,
                    AjustadoClearence = c.AjustadoClearence,
                    FechaInicio = c.FechaInicio,
                    FechaSuspension = c.FechaSuspension
                });

                var cultivos = await _infectoService.InfectologicoCultivoAsync(item.Id);
                new_item.Cultivos = cultivos.Select(c => new InfectologicoListModel()
                {
                    Id = c.CultivoId,
                    Nombre = c.Cultivo,
                    Resultado = c.Resultado,
                    Fecha = c.Fecha
                }).ToList();

                foreach (var cultivo_item in new_item.Cultivos)
                {
                    var items_sensibilidad = await ListSensibilidadCultivoAsync(item.Id, cultivo_item.Id);
                    cultivo_item.Sensibilidad = items_sensibilidad.ToList();
                }

                var hisopados = await _infectoService.HisopadoInfectologicoAsync(item.Id);
                new_item.Hisopados = hisopados.Select(c => new InfectologicoListModel()
                {
                    Nombre = c.Hisopado,
                    Resultado = c.Resultado,
                    Fecha = c.Fecha
                });

                model.Add(new_item);
            }

            return PartialView(model);
        }

        public async Task<PartialViewResult> _SNC(int PacienteId)
        {
            var items = await _sncService.ListSNCAsync(PacienteId);

            List<SNCFormModel> model = new List<SNCFormModel>();

            foreach (var item in items)
            {
                var new_item = new SNCFormModel()
                {
                    Id = item.Id,
                    Sedacion = item.Sedacion,
                    ValorSedacion = item.ValorSedacion,
                    SxAbstinencia = item.SxAbstinencia,
                    ConocidoConvulsionador = item.ConocidoConvulsionador,
                    Eventos = item.Eventos,
                    Planes = item.Planes,
                    UserName = item.UserAdd,
                    DateAdd = item.DateAdd
                };

                var asp_gral = await _sncService.SNCAspectoGralAsync(item.Id);
                new_item.AspectoGral = asp_gral.Select(c => new ListSNCModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                });

                var med_sedacion = await _sncService.SNCMedicamentoSedacionAsync(item.Id);
                new_item.MedicamentoSedacion = med_sedacion.Select(c => new ListSNCModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor
                });

                var lab = await _sncService.SNCLaboratorioAsync(item.Id);
                new_item.Laboratorio = lab.Select(c => new ListSNCModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Fecha = c.Fecha,
                    Valor = c.Valor
                });

                var img = await _sncService.SNCImagenesAsync(item.Id);
                new_item.Imagenes = img.Select(c => new ListSNCModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Fecha = c.Fecha,
                    Valor = c.Valor
                });

                var sx = await _sncService.SNCSxAbstinenciaAsync(item.Id);
                new_item.ListSxAbstinencia = sx.Select(c => new ListSNCModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor
                });

                var sx_med = await _sncService.SNCAbstinenciaMedicacionAsync(item.Id);
                new_item.ListSxAbstinenciaMedicacion = sx_med.Select(c => new ListSNCModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor
                });

                var med_conv = await _sncService.SNCAnticonvulsionanteAsync(item.Id);
                new_item.MedicamentoConvulsionador = med_conv.Select(c => new ListSNCModel()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Valor = c.Valor
                });

                model.Add(new_item);
            }

            return PartialView(model);
        }

        #region Ap Cardiovascular
        private string EstadoApCardiovascular(string EstadoId)
        {
            switch (EstadoId)
            {
                case "E":
                    return "Estable";
                case "I":
                    return "Inestable";
                case "L":
                    return "Lábil";
                default:
                    return "";
            }
        }

        private string CurvaEnzimas(string valor)
        {
            switch (valor)
            {
                case "N":
                    return "Normal";
                case "A":
                    return "Aumentado";
                case "D":
                    return "Disminuido";
                default:
                    return "";
            }
        }

        #endregion

        #region Infectológico

        private async Task<IEnumerable<SensibilidadModel>> ListSensibilidadCultivoAsync(int InfectologicoId, int CultivoId)
        {
            var items = await _infectoService.SensibilidadCultivoInfectologicoAsync(InfectologicoId, CultivoId);

            var model = items.Select(c => new SensibilidadModel()
            {
                Sensibilidad = c.Sensibilidad
            });

            return model;
        }

        #endregion

    }
}
