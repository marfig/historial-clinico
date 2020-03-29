function LoadApRespiratorio() {
    let paciente_id = $("#Id").val();

    $("#divApRespiratorio").load(`/Evolucion/_ApRespiratorio?PacienteId=${paciente_id}`, function () { SetEventosApResp(); });
}

function SetEventosApResp() {
    SetNumricAndDates();

    $("#modalApRespiratorio input[name='SoporteRespiratorio']").click(function () {
        SetearParametros($(this).data("parametros"));
    });
}

function NuevoApRespiratorio() {
    $("#modalApRespiratorio").modal("show");
}

function SetearParametros(parametros) {
    if (parametros === 0) {
        $("#modalApRespiratorio .parametros").hide();
        $("#modalApRespiratorio .valores").show();
    }

    if (parametros === 1) {
        $("#modalApRespiratorio .parametros").show();
        $("#modalApRespiratorio .valores").hide();
    }
}

function GuardarFormularioApRespiratorio(button) {
    if (!ValidarFormApRespiratorio()) {
        return false;
    }
    
    let param_soporte = GetSoporteRespiratorioParamJSON();
    let param_gasom = GetGasometriaParamJSON();

    let form_json = {
        PacienteId: $("#Id").val(),
        SoporteRespiratorioId: $("#modalApRespiratorio input[name='SoporteRespiratorio']:checked").val(),
        ValorSoporteResp: $("#modalApRespiratorio #ValorSopResp").val(),
        SoporteRespiratorioParamJSON: param_soporte,
        VentilacionId: $("#modalApRespiratorio #ddlVentilacion").val(),
        ModalidadId: $("#modalApRespiratorio #ddlModalidad").val(),
        GasometriaId: $("#modalApRespiratorio #ddlGasometria").val(),
        GasometriaParamJSON: param_gasom,
        Manejo: $("#modalApRespiratorio #Manejo").val(),
        Eventos: $("#modalApRespiratorio #Eventos").val(),
        Planes: $("#modalApRespiratorio #Planes").val()
    };

    $(button).button('loading');

    $.ajax({
        method: 'POST',
        url: '/Evolucion/AddApRespiratorio',
        data: form_json
    })
        .done(function (data) {
            if (data.Success) {
                $("#modalApRespiratorio").modal("toggle");
                LoadApRespiratorio();
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $(button).button('reset');
        });
        
}

function ValidarFormApRespiratorio() {
    let isValid = true;

    $("#modalApRespiratorio input[name='SoporteRespiratorio']").closest(".form-group").find("em").remove();

    if ($("#modalApRespiratorio input[name='SoporteRespiratorio']:checked").length === 0) {
        $("#modalApRespiratorio input[name='SoporteRespiratorio']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar el Soporte Respiratorio</em>`);
        isValid = false;
    }

    if ($("#modalApRespiratorio #ddlGasometria").val().length === 0) {
        $("#modalApRespiratorio #ddlGasometria").closest(".form-group").addClass("has-error");
        $("#modalApRespiratorio #ddlGasometria").closest(".form-group").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalApRespiratorio #ddlGasometria").closest(".form-group").removeClass("has-error");
        $("#modalApRespiratorio #ddlGasometria").closest(".form-group").addClass("has-success");
    }


    if ($("#modalApRespiratorio #Manejo").val().length === 0) {
        $("#modalApRespiratorio #Manejo").closest(".form-group").addClass("has-error");
        $("#modalApRespiratorio #Manejo").closest(".form-group").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalApRespiratorio #Manejo").closest(".form-group").removeClass("has-error");
        $("#modalApRespiratorio #Manejo").closest(".form-group").addClass("has-success");
    }

    return isValid;
}

function GetSoporteRespiratorioParamJSON() {
    let parametros = [];

    $("#tblSopRespParam tbody tr").each(function () {
        let id = $(this).data("id");
        let input_valor = $(this).find("input[name='valor_parametro']");
        let valor = $(input_valor).val().trim();

        if (valor.length > 0) {
            parametros.push({ Id: id, Valor: valor });
        }
    });

    if (parametros.length === 0) {
        return '';
    }

    return JSON.stringify(parametros);
}

function GetGasometriaParamJSON() {
    let parametros = [];

    $("#tblGasometriaParam tbody tr").each(function () {
        let id = $(this).data("id");
        let input_valor = $(this).find("input[name='valor_gasometria']");
        let valor = $(input_valor).val().trim();

        if (valor.length > 0) {
            parametros.push({ Id: id, Valor: valor });
        }
    });

    if (parametros.length === 0) {
        return '';
    }

    return JSON.stringify(parametros);
}