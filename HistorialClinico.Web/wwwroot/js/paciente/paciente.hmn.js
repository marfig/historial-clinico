function LoadHMN() {
    let paciente_id = $("#Id").val();

    $("#divHMN").load(`/Evolucion/_HMN?PacienteId=${paciente_id}`, function () { SetEventosHMN(); });
}

function SetEventosHMN() {
    SetNumricAndDates();

    $("#modalHMN input[name='chk_dialisis_peritoneal']").change(function () {
        ConfigurarDivDialisisFormulacion(this);
        return false;
    });
}

function ConfigurarDivDialisisFormulacion(item) {
    if ($(item).val() === 'S') {
        $("#modalHMN #divDialisisFormulacion").show();
    }

    if ($(item).val() === 'N') {
        $("#modalHMN #divDialisisFormulacion").hide();
    }
}

function NuevoHMN() {
    $("#modalHMN").modal("show");
}

function GuardarFormularioHMN(button) {
    let general_valid = GetGeneralHMNJSON();

    if (!general_valid) {
        return false;
    }

    GetBalanceHidricoJSON();
    GetLaboratorioHMNJSON();

    let dialisis_peritoneal = $("#modalHMN input[name='chk_dialisis_peritoneal']:checked").val() === "S";
    
    let form_json = {
        PacienteId: $("#Id").val(),
        DialisisPeritoneal: dialisis_peritoneal,
        FormulacionDialisisPeritoneal: $("#modalHMN #FormulacionDialisisPeritoneal").val(),
        GeneralJSON: $("#modalHMN #GeneralJSON").val(),
        BalanceHidricoJSON: $("#modalHMN #BalanceHidricoJSON").val(),
        LaboratorioJSON: $("#modalHMN #LaboratorioHMNJSON").val(),
        Eventos: $("#modalHMN #Eventos").val(),
        Planes: $("#modalHMN #Planes").val()
    };

    $(button).button('loading');

    $.ajax({
        method: 'POST',
        url: '/Evolucion/AddHMN',
        data: form_json
    })
        .done(function (data) {
            if (data.Success) {
                $("#modalHMN").modal("toggle");
                LoadHMN();
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $(button).button('reset');
        });
}

function GetGeneralHMNJSON() {
    let parametros = [];
    let isValid = true;
    

    $("#tblHMNGral tbody tr").each(function () {
        let input_check = $(this).find("input[type='checkbox']");
        let check = $(input_check).prop("checked");
        let id = $(input_check).val();
        let formulacion_valor = '';
        let valor = '';

        $(valor).closest("td").removeClass("has-error");

        if (check) {

            let formulacion = $(this).data("formulacion").toLowerCase();

            if (JSON.parse(formulacion)) {
                let input_formulacion = $(this).find("input[name='gral_formulacion']");
                formulacion_valor = $(input_formulacion).val();

                if ($(input_formulacion).val().trim().length === 0) {
                    $(input_formulacion).closest("td").addClass("has-error");
                    isValid = false;
                }
            }
            else {
                let input_valor = $(this).find("input[name='gral_valor']");
                valor = $(input_valor).val();

                if ($(input_valor).val().trim().length === 0) {
                    $(input_valor).closest("td").addClass("has-error");
                    isValid = false;
                }
            }
        }

        if (check && isValid) {
            parametros.push({
                Id: id,
                Valor: valor,
                Formulacion: formulacion_valor
            });
        }
    });

    if (parametros.length === 0) {
        $("#GeneralJSON").val('');
    }

    $("#GeneralJSON").val(JSON.stringify(parametros));

    return isValid;
}

function GetBalanceHidricoJSON() {
    let parametros = [];
    let isValid = true;

    $("#tblBalanceHidrico tbody tr").each(function () {
        let id = $(this).data("id");
        let valor = $(this).find("input[name='balance_hidrico_valor']");

        if ($(valor).val().trim().length > 0) {
            parametros.push({
                Id: id,
                Valor: $(valor).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#BalanceHidricoJSON").val('');
    }

    $("#BalanceHidricoJSON").val(JSON.stringify(parametros));

    return isValid;
}

function GetLaboratorioHMNJSON() {
    let parametros = [];
    let isValid = true;

    $("#tblLaboratorioHMN tbody tr").each(function () {
        let id = $(this).data("id");

        if (Numeric(id) > 0) {

            let valor = $(this).find("input[name='valor_lab']");

            if ($(valor).val().trim().length > 0) {
                parametros.push({
                    Id: id,
                    Valor: $(valor).val()
                });
            }
        }
    });

    if (parametros.length === 0) {
        $("#LaboratorioHMNJSON").val('');
    }

    $("#LaboratorioHMNJSON").val(JSON.stringify(parametros));

    return isValid;
}