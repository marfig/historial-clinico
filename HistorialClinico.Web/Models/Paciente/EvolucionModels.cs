using System;
using System.Collections.Generic;

namespace HistorialClinico.Web.Models.Paciente
{
    #region ApCardioVascular
    public class ApCardiovascularFormModel
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string Estado { get; set; }
        public IEnumerable<InotropicosModel> Inotropicos { get; set; }
        public string InotropicosJSON { get; set; }
        public string EvaluacionCardiologica { get; set; }
        public IEnumerable<EnzimasCardiacasModel> EnzimasCardiacas { get; set; }
        public string EnzimasCardiacasJSON { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
        public DateTime DateAdd { get; set; }
    }

    public class InotropicosModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
    }

    public class EnzimasCardiacasModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Valor { get; set; }
        public string Curva { get; set; }
    }
    #endregion

    #region ApRespiratorio

    public class ApRespiratorioFormModel
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int? SoporteRespiratorioId { get; set; }
        public string SoporteRespiratorio { get; set; }
        public bool Parametros { get; set; }
        public string ValorSoporteResp { get; set; }
        public IEnumerable<ParametrosApRespModel> SoporteRespiratorioParam { get; set; }
        public string SoporteRespiratorioParamJSON { get; set; }
        public string Ventilacion { get; set; }
        public int? VentilacionId { get; set; }
        public string Modalidad { get; set; }
        public int? ModalidadId { get; set; }
        public string Gasometria { get; set; }
        public int? GasometriaId { get; set; }
        public IEnumerable<ParametrosApRespModel> GasometriaParam { get; set; }
        public string GasometriaParamJSON { get; set; }
        public string Manejo { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
        public DateTime DateAdd { get; set; }
    }

    public class ParametrosApRespModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Valor { get; set; }
    }

    public class SoporteRespModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Parametros { get; set; }
    }

    #endregion

    #region Infectologico

    public class InfectologicoFormModel
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int? EstadoInfectologicoId { get; set; }
        public string EstadoInfectologico { get; set; }
        public IEnumerable<CoberturaAtbModel> CoberturaAtb { get; set; }
        public string CoberturaAtbJSON { get; set; }
        public IEnumerable<InfectologicoListModel> Cultivos { get; set; }
        public string CultivosJSON { get; set; }
        public IEnumerable<SensibilidadModel> Sensibilidad { get; set; }
        public string SensibilidadJSON { get; set; }
        public IEnumerable<InfectologicoListModel> Hisopados { get; set; }
        public string HisopadosJSON { get; set; }
        public string Interconsulta { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
        public DateTime DateAdd { get; set; }
    }

    public class CoberturaAtbModel
    {
        public string Antibiotico { get; set; }
        public decimal Dosis { get; set; }
        public string Unidad { get; set; }
        public bool AjustadoClearence { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaSuspension { get; set; }
    }

    public class InfectologicoListModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Resultado { get; set; }
        public DateTime Fecha { get; set; }
        public IEnumerable<SensibilidadModel> Sensibilidad { get; set; }
    }

    public class SensibilidadModel
    {
        public string Sensibilidad { get; set; }
    }


    #endregion

    #region SNC

    public class SNCFormModel
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public IEnumerable<ListSNCModel> AspectoGral { get; set; }
        public string Sedacion { get; set; }
        public decimal ValorSedacion { get; set; }
        public IEnumerable<ListSNCModel> MedicamentoSedacion { get; set; }
        public IEnumerable<ListSNCModel> Laboratorio { get; set; }
        public IEnumerable<ListSNCModel> Imagenes { get; set; }
        public bool SxAbstinencia { get; set; }
        public IEnumerable<ListSNCModel> ListSxAbstinencia { get; set; }
        public IEnumerable<ListSNCModel> ListSxAbstinenciaMedicacion { get; set; }
        public bool ConocidoConvulsionador { get; set; }
        public IEnumerable<ListSNCModel> MedicamentoConvulsionador { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
        public DateTime DateAdd { get; set; }
    }

    public class ListSNCModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? Valor { get; set; }
    }

    #endregion

    #region HMN

    public class HMNFormModel
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public bool DialisisPeritoneal { get; set; }
        public string FormulacionDialisisPeritoneal { get; set; }
        public IEnumerable<ListHMNModel> General { get; set; }
        public IEnumerable<ListHMNModel> BalanceHidrico { get; set; }
        public IEnumerable<ListHMNModel> Laboratorio { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
        public DateTime DateAdd { get; set; }
    }

    public class ListHMNModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Valor { get; set; }
        public string Formulacion { get; set; }
        public string Categoria { get; set; }
    }

    #endregion

    #region Hematologico

    public class HematologicoFormModel
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public decimal? Hemograma_PAI { get; set; }
        public decimal? Hemograma_HB { get; set; }
        public decimal? Hemograma_HTC { get; set; }
        public decimal? Hemograma_PLT { get; set; }
        public decimal? Crasis_TP { get; set; }
        public decimal? Crasis_TTPA { get; set; }
        public decimal? Crasis_Fibrinoginos { get; set; }
        public bool VitaminaK { get; set; }
        public decimal? DosisVitaminaK { get; set; }
        public DateTime? FechaVitaminaK { get; set; }
        public bool SangradoActivo { get; set; }
        public string LugarSangrado { get; set; }
        public bool Transfusiones { get; set; }
        public decimal? Transfusiones_GRC { get; set; }
        public decimal? Transfusiones_PFC { get; set; }
        public decimal? Transfusiones_CRIO { get; set; }
        public decimal? Transfusiones_PLT { get; set; }
        public string Eventos { get; set; }
        public string Planes { get; set; }
        public string UserName { get; set; }
        public DateTime DateAdd { get; set; }
    }

    #endregion
}
