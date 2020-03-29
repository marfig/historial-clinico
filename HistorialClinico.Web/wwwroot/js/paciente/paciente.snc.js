function LoadSNC() {
    let paciente_id = $("#Id").val();

    $("#divSNC").load(`/Evolucion/_SNC?PacienteId=${paciente_id}`, function () { SetEventosSNC(); });
}

function SetEventosSNC() {
    SetNumricAndDates();

    $("#modalSNC input[name='rb_sx_abstinencia']").change(function () {
        ConfigurarDivSxAbstinencia(this);
        return false;
    });

    $("#modalSNC input[name='rb_sedacion']").change(function () {
        ConfigurarDivSedacion(this);
        return false;
    });
}

function NuevoSNC() {
    $("#modalSNC").modal("show");
}

function ConfigurarDivSxAbstinencia(item) {
    if ($(item).val() === 'S') {
        $("#modalSNC #divSxAbstinencia").show();
    }

    if ($(item).val() === 'N') {
        $("#modalSNC #divSxAbstinencia").hide();
    }
}

function ConfigurarDivSedacion(item) {
    let show = $(item).data("requiere-valor").toLowerCase();

    if (JSON.parse(show) === true) {
        $("#modalSNC #divSedacion").show();
    }

    if (JSON.parse(show) === false) {
        $("#modalSNC #divSedacion").hide();
    }
}

function GuardarFormularioSNC(button) {
    let laboratorio_valid = GetLaboratorioJSON();
    let imagenes_valid = GetImagenesJSON();

    if (!ValidarFormSNC() || !laboratorio_valid || !imagenes_valid) {
        return false;
    }

    GetAspectoGralJSON();
    GetSedacionJSON();
    GetSxAbstinenciaJSON();
    GetSxAbstinenciaMedicacionJSON();
    GetConvulsionadorJSON();

    let sx_abs = ($("#modalSNC input[name='rb_sx_abstinencia']:checked").val() === "S");
    let conv = ($("#modalSNC input[name='rb_convulsionador']:checked").val() === "S");

    let form_json = {
        PacienteId: $("#Id").val(),
        AspectoGralJSON: $("#modalSNC #AspectoGralJSON").val(),
        SedacionId: $("#modalSNC input[name='rb_sedacion']:checked").val(),
        SedacionMedicamentoJSON: $("#modalSNC #SedacionMedicamentoJSON").val(),
        ValorSedacion: $("#modalSNC #txtValorSedacion").val(),
        LaboratorioJSON: $("#modalSNC #LaboratorioJSON").val(),
        ImagenesJSON: $("#modalSNC #ImagenesJSON").val(),
        SxAbstinenciaId: sx_abs,
        SxAbstinenciaJSON: $("#modalSNC #SxAbstinenciaJSON").val(),
        SxAbstinenciaMedicacionJSON: $("#modalSNC #SxAbstinenciaMedicacionJSON").val(),
        ConocidoConvulsionadorId: conv,
        ConocidoConvulsionadorJSON: $("#modalSNC #ConocidoConvulsionadorJSON").val(),
        Eventos: $("#modalSNC #Eventos").val(),
        Planes: $("#modalSNC #Planes").val()
    };

    $(button).button('loading');

    $.ajax({
        method: 'POST',
        url: '/Evolucion/AddSNC',
        data: form_json
    })
        .done(function (data) {
            if (data.Success) {
                $("#modalSNC").modal("toggle");
                LoadSNC();
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $(button).button('reset');
        });
}

function ValidarFormSNC() {
    let isValid = true;

    $("#modalSNC input[name='rb_sedacion']").closest(".form-group").find("em").remove();

    if ($("#modalSNC input[name='rb_sedacion']:checked").length === 0) {
        $("#modalSNC input[name='rb_sedacion']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar la Sedación</em>`);
        isValid = false;
    }

    $("#modalSNC input[name='rb_sx_abstinencia']").closest(".form-group").find("em").remove();

    if ($("#modalSNC input[name='rb_sx_abstinencia']:checked").length === 0) {
        $("#modalSNC input[name='rb_sx_abstinencia']").closest(".form-group").append(`<em style="color: #b94a48">Debe indicar Abstinencia</em>`);
        isValid = false;
    }

    $("#modalSNC input[name='rb_convulsionador']").closest(".form-group").find("em").remove();

    if ($("#modalSNC input[name='rb_convulsionador']:checked").length === 0) {
        $("#modalSNC input[name='rb_convulsionador']").closest(".form-group").append(`<em style="color: #b94a48">Debe indicar si es conocido Convulsionador</em>`);
        isValid = false;
    }

    return isValid;
}

function GetAspectoGralJSON() {
    let parametros = [];

    $("#modalSNC input[name='chk_aspecto_gral']:checked").each(function () {
        let id = $(this).val();

        parametros.push({
            Id: id
        });
    });

    if (parametros.length === 0) {
        $("#AspectoGralJSON").val('');
    }

    $("#AspectoGralJSON").val(JSON.stringify(parametros));
}

function GetSedacionJSON() {
    let parametros = [];

    $("#tblMedicamentoSedacion tbody tr").each(function () {
        let id = $(this).data("id");
        let dosis = $(this).find("input[name='dosis_sedacion']");

        if ($(dosis).val().trim().length > 0) {
            parametros.push({
                Id: id,
                Valor: $(dosis).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#SedacionMedicamentoJSON").val('');
    }

    $("#SedacionMedicamentoJSON").val(JSON.stringify(parametros));
}

function GetLaboratorioJSON() {
    let parametros = [];
    let isValid = true;

    $("#tblLaboratorio tbody tr").each(function () {
        let id = $(this).data("id");
        let fecha = $(this).find("input[name='fecha_laboratorio']");
        let valor = $(this).find("input[name='valor_laboratorio']");

        $(fecha).closest("td").removeClass("has-error");
        $(valor).closest("td").removeClass("has-error");

        if ($(fecha).val().trim().length > 0) {
            if ($(valor).val().trim().length === 0) {
                $(valor).closest("td").addClass("has-error");
                isValid = false;
            }
        }

        if ($(valor).val().trim().length > 0) {
            if ($(fecha).val().trim().length === 0) {
                $(fecha).closest("td").addClass("has-error");
                isValid = false;
            }
        }

        if ($(valor).val().trim().length > 0 && $(fecha).val().trim().length > 0) {
            parametros.push({
                Id: id,
                Fecha: $(fecha).val(),
                Valor: $(valor).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#LaboratorioJSON").val('');
    }

    $("#LaboratorioJSON").val(JSON.stringify(parametros));

    return isValid;
}

function GetImagenesJSON() {
    let parametros = [];
    let isValid = true;

    $("#tblImagenes tbody tr").each(function () {
        let id = $(this).data("id");
        let fecha = $(this).find("input[name='fecha_img']");
        let valor = $(this).find("input[name='valor_img']");

        $(fecha).closest("td").removeClass("has-error");
        $(valor).closest("td").removeClass("has-error");

        if ($(fecha).val().trim().length > 0) {
            if ($(valor).val().trim().length === 0) {
                $(valor).closest("td").addClass("has-error");
                isValid = false;
            }
        }

        if ($(valor).val().trim().length > 0) {
            if ($(fecha).val().trim().length === 0) {
                $(fecha).closest("td").addClass("has-error");
                isValid = false;
            }
        }

        if ($(valor).val().trim().length > 0 && $(fecha).val().trim().length > 0) {
            parametros.push({
                Id: id,
                Fecha: $(fecha).val(),
                Valor: $(valor).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#ImagenesJSON").val('');
    }

    $("#ImagenesJSON").val(JSON.stringify(parametros));

    return isValid;
}

function GetSxAbstinenciaJSON() {
    let parametros = [];

    $("#tblSxAbstinencia tbody tr").each(function () {
        let id = $(this).data("id");
        let valor = $(this).find("input[name='valor_sx']");

        if ($(valor).val().trim().length > 0) {
            parametros.push({
                Id: id,
                Valor: $(valor).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#SxAbstinenciaJSON").val('');
    }

    $("#SxAbstinenciaJSON").val(JSON.stringify(parametros));
}

function GetSxAbstinenciaMedicacionJSON() {
    let parametros = [];

    $("#tblSxAbstinenciaMedicacion tbody tr").each(function () {
        let id = $(this).data("id");
        let valor = $(this).find("input[name='dosis_sx']");

        if ($(valor).val().trim().length > 0) {
            parametros.push({
                Id: id,
                Valor: $(valor).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#SxAbstinenciaMedicacionJSON").val('');
    }

    $("#SxAbstinenciaMedicacionJSON").val(JSON.stringify(parametros));
}

function GetConvulsionadorJSON() {
    let parametros = [];

    $("#tblConvulsionador tbody tr").each(function () {
        let id = $(this).data("id");
        let valor = $(this).find("input[name='dosis_conv']");

        if ($(valor).val().trim().length > 0) {
            parametros.push({
                Id: id,
                Valor: $(valor).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#ConocidoConvulsionadorJSON").val('');
    }

    $("#ConocidoConvulsionadorJSON").val(JSON.stringify(parametros));
}
