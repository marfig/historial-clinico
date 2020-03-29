#region Using

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HistorialClinico.Web.Models.Paciente;
using System.Collections.Generic;
using HistorialClinico.Services.Interfaces;
using System.Threading.Tasks;
using HistorialClinico.Domain.DTO;
using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion

namespace HistorialClinico.Web.Controllers
{
    [Authorize]
    public class EvolucionController : Controller
    {
        #region Inicio
        private readonly IApCardiovascularService _apCardioService;
        private readonly IApRespiratorioService _apRespService;
        private readonly IInfectologicoService _infectoService;
        private readonly ISNCService _sncService;
        private readonly IHMNService _hmnService;
        private readonly IHematologicoService _hematologicoService;
        private readonly IFichaPacienteService _fichaPacienteService;
        private readonly ICirugiaService _cirugiaService;

        public EvolucionController(IApCardiovascularService apCardioService,
                                   IApRespiratorioService apRespService,
                                   IInfectologicoService infectoService,
                                   ISNCService sncService,
                                   IHMNService hmnService,
                                   IHematologicoService hematologicoService,
                                   IFichaPacienteService fichaPacienteService,
                                   ICirugiaService cirugiaService)
        {
            _apCardioService = apCardioService;
            _apRespService = apRespService;
            _infectoService = infectoService;
            _sncService = sncService;
            _hmnService = hmnService;
            _hematologicoService = hematologicoService;
            _fichaPacienteService = fichaPacienteService;
            _cirugiaService = cirugiaService;
        }
        #endregion

        #region Actions
        public IActionResult Index()
        {
            return View();
        }

        #region ApCardiovascular
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

            ViewBag.ListInotropicos = await SelectListInotropicosAsync(0);
            ViewBag.ListEnzimas = await SelectListEnzimasAsync(0);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> ListInotropicos(int ApCardiovascularId)
        {
            var items = await SelectListInotropicosAsync(ApCardiovascularId);

            return Json(new { Success = true, Listado = items });
        }

        [HttpPost]
        public async Task<JsonResult> ListEnzimasCardiacas(int ApCardiovascularId)
        {
            var items = await SelectListEnzimasAsync(ApCardiovascularId);

            return Json(new
            {
                Success = true,
                Listado = items
            });
        }

        [HttpPost]
        public async Task<JsonResult> AddApCardiovascular(ApCardiovascularFormModel model)
        {
            await _apCardioService.AddApCardiovascularAsync(model.PacienteId, model.Estado, model.EvaluacionCardiologica, model.InotropicosJSON, model.EnzimasCardiacasJSON, model.Eventos, model.Planes, User.Identity.Name);

            return Json(new { Success = true });
        }
        #endregion

        #region ApRespiratorio

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

            ViewBag.ListSoporteRespiratorio = await SelectSoporteRespiratorioAsync();
            ViewBag.ListVentilacion = await SelectVentilacionAsync();
            ViewBag.ListModalidad = await SelectModalidadAsync();
            ViewBag.ListSopParametros = await SelectSoporteRespParametrosAsync();
            ViewBag.ListGasometria = await SelectGasometriaAsync();
            ViewBag.ListGasometriaParametros = await SelectGasometriaParametrosAsync();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddApRespiratorio(ApRespiratorioFormModel model)
        {
            var item_dto = Mapper.Map<ApRespiratorioDTO>(model);

            item_dto.UserName = User.Identity.Name;

            await _apRespService.AddApRespiratorioAsync(item_dto);

            return Json(new { Success = true });
        }

        #endregion

        #region Infectológico

        public async Task<PartialViewResult> _Infectologico(int PacienteId)
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


            ViewBag.ListEstados = await SelectEstadoInfectologicoAsync();
            ViewBag.ListCultivos = await SelectCultivoAsync();
            ViewBag.ListHisopados = await SelectHisopadoAsync();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddInfectologico(InfectologicoDTO model)
        {
            model.UserName = User.Identity.Name;

            await _infectoService.AddInfectologicoAsync(model);

            return Json(new { Success = true });
        }

        #endregion

        #region SNC

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

            ViewBag.ListAspectoGral = await SelectAspectoGralAsync();
            ViewBag.ListSedacion = await SelectSedacionAsync();
            ViewBag.ListMedicamentoSedacion = await SelectMedicamentoSedacionAsync();
            ViewBag.ListLaboratorio = await SelectLaboratorioAsync();
            ViewBag.ListImagenes = await SelectImagenesAsync();
            ViewBag.ListSxAbstinencia = await SelectSxAbstinenciaAsync();
            ViewBag.ListSxAbstinenciaMedicacion = await SelectMedicacionSNCAsync();
            ViewBag.ListAnticonvulsionante = await SelectAnticonvulsionanteAsync();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddSNC(SncDTO model)
        {
            model.UserName = User.Identity.Name;

            await _sncService.AddSNCAsync(model);

            return Json(new { Success = true });
        }

        #endregion

        #region HMN

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

            ViewBag.ListGeneralHMN = await SelectGeneralMHNAsync();
            ViewBag.ListBalanceHidrico = await SelectBalanceHidricoAsync();
            var list_lab = await _hmnService.ListLaboratorioHMNAsync();
            ViewBag.ListLabHMN = list_lab.Select(c => new ListHMNModel()
                                                            {
                                                                Id = c.Id,
                                                                Nombre = c.Nombre,
                                                                Categoria = c.Categoria
                                                            });

            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddHMN(HmnDTO model)
        {
            model.UserName = User.Identity.Name;

            await _hmnService.AddHMNAsync(model);

            return Json(new { Success = true });
        }

        #endregion

        #region Hematológico

        public async Task<PartialViewResult> _Hematologico(int PacienteId)
        {
            var paciente = await _fichaPacienteService.GetDatosBasicosByIdAsync(PacienteId);
            var items = await _hematologicoService.ListHematologicoAsync(PacienteId);

            ViewBag.GrupoSanguineo = paciente.GrupoSanguineo;
            
            var model = Mapper.Map<List<HematologicoFormModel>>(items);
            
            
            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddHematologico(HematologicoDTO model)
        {
            model.UserName = User.Identity.Name;

            await _hematologicoService.AddHematologicoAsync(model);

            return Json(new { Success = true });
        }

        #endregion

        #region Cirugía

        public async Task<PartialViewResult> _Cirugia(int PacienteId)
        {
            var items = await _cirugiaService.ListCirugiaAsync(PacienteId);

            var model = Mapper.Map<List<CirugiaDTO>>(items);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddCirugia(CirugiaDTO model)
        {
            model.UserAdd = User.Identity.Name;

            await _cirugiaService.AddCirugiaAsync(model);

            return Json(new { Success = true });
        }

        #endregion

        #endregion

        #region Helpers

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

        private async Task<IEnumerable<SelectListItem>> SelectListInotropicosAsync(int ApCardiovascularId)
        {
            var items = await _apCardioService.ListInotropicosAsync(ApCardiovascularId, false);
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectListEnzimasAsync(int ApCardiovascularId)
        {
            var items = await _apCardioService.ListEnzimasCardiacasAsync(ApCardiovascularId);
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }
        #endregion

        #region Ap Respiratorio
        private async Task<IEnumerable<SelectListItem>> SelectVentilacionAsync()
        {
            var items = await _apRespService.ListVentilacionAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectModalidadAsync()
        {
            var items = await _apRespService.ListModalidadAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SoporteRespModel>> SelectSoporteRespiratorioAsync()
        {
            var items = await _apRespService.ListSoporteRespiratorioAsync();
            var model = items.Select(c => new SoporteRespModel()
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Parametros = c.Parametros ? 1 : 0
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectSoporteRespParametrosAsync()
        {
            var items = await _apRespService.ListSoporteRespParametrosAsync(0);
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectGasometriaAsync()
        {
            var items = await _apRespService.ListGasometriaAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectGasometriaParametrosAsync()
        {
            var items = await _apRespService.ListGasometriaParametrosAsync(0);
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
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

        private async Task<IEnumerable<SelectListItem>> SelectEstadoInfectologicoAsync()
        {
            var items = await _infectoService.ListEstadoInfectologicoAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectCultivoAsync()
        {
            var items = await _infectoService.ListCultivoAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectHisopadoAsync()
        {
            var items = await _infectoService.ListHisopadoAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        #endregion

        #region SNC
        private async Task<IEnumerable<SelectListItem>> SelectAspectoGralAsync()
        {
            var items = await _sncService.ListAspectoGralAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectSedacionAsync()
        {
            var items = await _sncService.ListSedacionAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre,
                Selected = c.RequeridoValor
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectMedicamentoSedacionAsync()
        {
            var items = await _sncService.ListMedicamentoSedacionAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectLaboratorioAsync()
        {
            var items = await _sncService.ListLaboratorioSNCAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectImagenesAsync()
        {
            var items = await _sncService.ListImagenSNCAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectSxAbstinenciaAsync()
        {
            var items = await _sncService.ListSxAbstinenciaSNCAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectMedicacionSNCAsync()
        {
            var items = await _sncService.ListMedicacionSNCAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectAnticonvulsionanteAsync()
        {
            var items = await _sncService.ListAnticonvulsionanteAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }
        #endregion

        #region HMN
        private async Task<IEnumerable<SelectListItem>> SelectGeneralMHNAsync()
        {
            var items = await _hmnService.ListGeneralHMNAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre,
                Selected = c.Formulacion
            });

            return model;
        }

        private async Task<IEnumerable<SelectListItem>> SelectBalanceHidricoAsync()
        {
            var items = await _hmnService.ListBalanceHidricoHMNAsync();
            var model = items.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });

            return model;
        }
        #endregion
        #endregion
    }
}