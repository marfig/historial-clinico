function LoadInfectologico() {
    let paciente_id = $("#Id").val();

    $("#divInfectologico").load(`/Evolucion/_Infectologico?PacienteId=${paciente_id}`, function () { SetEventosInfecto(); });
}

function SetEventosInfecto() {
    SetNumricAndDates();
}

function NuevoInfectologico() {
    $("#modalInfectologico").modal("show");
}

function AddCoberturaAtb() {
    let isValid = true;
    let nombre_atb = '';
    let dosis = 0;
    let unidad = '';
    let fecha_inicio = '';
    let ajustado_clr = $("#modalInfectologico #ajustado_clr").prop("checked");
    let fecha_suspension = $("#modalInfectologico #fecha_suspension").val();

    if ($("#modalInfectologico #nombre_atb").val().length === 0) {
        $("#modalInfectologico #nombre_atb").closest("td").addClass("has-error");
        $("#modalInfectologico #nombre_atb").closest("td").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalInfectologico #nombre_atb").closest("td").removeClass("has-error");
        $("#modalInfectologico #nombre_atb").closest("td").addClass("has-success");
        nombre_atb = $("#modalInfectologico #nombre_atb").val();
    }


    if ($("#modalInfectologico #dosis").val().length === 0) {
        $("#modalInfectologico #dosis").closest("td").addClass("has-error");
        $("#modalInfectologico #dosis").closest("td").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalInfectologico #dosis").closest("td").removeClass("has-error");
        $("#modalInfectologico #dosis").closest("td").addClass("has-success");
        dosis = $("#modalInfectologico #dosis").val();
    }

    if ($("#modalInfectologico #unidad").val().length === 0) {
        $("#modalInfectologico #unidad").closest("td").addClass("has-error");
        $("#modalInfectologico #unidad").closest("td").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalInfectologico #unidad").closest("td").removeClass("has-error");
        $("#modalInfectologico #unidad").closest("td").addClass("has-success");
        unidad = $("#modalInfectologico #unidad").val();
    }

    if ($("#modalInfectologico #fecha_inicio").val().length === 0) {
        $("#modalInfectologico #fecha_inicio").closest("td").addClass("has-error");
        $("#modalInfectologico #fecha_inicio").closest("td").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalInfectologico #fecha_inicio").closest("td").removeClass("has-error");
        $("#modalInfectologico #fecha_inicio").closest("td").addClass("has-success");
        fecha_inicio = $("#modalInfectologico #fecha_inicio").val();
    }

    let ajustado_clr_text = 'NO';

    if (ajustado_clr) {
        ajustado_clr_text = 'SÍ';
    }

    if (isValid) {
        let fila = `<tr data-nombre-atb="${nombre_atb}" data-dosis="${dosis}" data-unidad="${unidad}" data-fecha-inicio="${fecha_inicio}" data-ajustado-clr="${ajustado_clr}" data-fecha-suspension="${fecha_suspension}">
						<td><span>${nombre_atb}</span></td>
						<td><span>${dosis}</span></td>
                        <td><span>${unidad}</span></td>
                        <td><span>${fecha_inicio}</span></td>
                        <td><span>${ajustado_clr_text}</span></td>
                        <td><span>${fecha_suspension}</span></td>
						<td class="col-md-1 text-center" style="vertical-align: middle">
							<a href="javascript:void('0')" onclick="RemoveCoberturaAtb(this)"><i class="glyphicon glyphicon-remove"></i></a>
						</td>
					</tr>`;

        $("#tblCoberturaAtb tbody").append(fila);

        $("#modalInfectologico #nombre_atb").val('');
        $("#modalInfectologico #dosis").val('');
        $("#modalInfectologico #unidad").val('');
        $("#modalInfectologico #fecha_inicio").val('');
        $("#modalInfectologico #ajustado_clr").prop("checked", false);
        $("#modalInfectologico #fecha_suspension").val('');
        
    }
}

function AddSensibilidad(item) {
    let fila = $(item).closest("tr");
    let id = $(fila).data("cultivo-id");
    let celda = $(item).closest("td");
    let input = $(celda).find("input[name='sensibilidad']");
    let valor = $(input).val();

    if (valor.trim().length > 0) {
        
        let lista = $(celda).find(".list-sensibilidad");
        $(lista).append(`<li data-valor="${valor}" data-cultivo-id="${id}">${valor} 
                            <a href="javascript:void('0')" onclick="RemoveSensibilidad(this)">
                                <i class="glyphicon glyphicon-remove"></i>
                            </a>
                        </li>`);
        $(input).val('');
    }
}
function RemoveSensibilidad(item) {
    let fila = $(item).closest("li");
    $(fila).remove();
} 

function RemoveCoberturaAtb(item) {
    let fila = $(item).closest("tr");
    $(fila).remove();
}

function GetCoberturaAtbJSON() {
    let parametros = [];
    
    $("#tblCoberturaAtb tbody tr").each(function () {
        let nombre_atb = $(this).data("nombre-atb");
        let dosis = $(this).data("dosis");
        let unidad = $(this).data("unidad");
        let fecha_inicio = $(this).data("fecha-inicio");
        let ajustado_clr = $(this).data("ajustado-clr");
        let fecha_suspension = $(this).data("fecha-suspension");

        parametros.push({
            Antibiotico: nombre_atb,
            Dosis: dosis,
            Unidad: unidad,
            FechaInicio: fecha_inicio,
            AjustadoClearence: ajustado_clr,
            FechaSuspension: fecha_suspension
        });
    });

    if (parametros.length === 0) {
        return '';
    }

    return JSON.stringify(parametros);
}

function GetCultivosJSON() {
    let parametros = [];
    let isValid = true;

    $("#tblCultivos tbody tr").each(function () {
        let id = $(this).data("cultivo-id");
        let resultado_cultivo = $(this).find("select[name='resultado_cultivo']");
        let fecha_cultivo = $(this).find("input[name='fecha_cultivo']");

        $(fecha_cultivo).closest("td").removeClass("has-error");
        $(resultado_cultivo).closest("td").removeClass("has-error");

        if ($(resultado_cultivo).val().trim().length > 0) {
            if ($(fecha_cultivo).val().trim().length === 0) {
                $(fecha_cultivo).closest("td").addClass("has-error");
                isValid = false;
            }
        }

        if ($(fecha_cultivo).val().trim().length > 0) {
            if ($(resultado_cultivo).val().trim().length === 0) {
                $(resultado_cultivo).closest("td").addClass("has-error");
                isValid = false;
            }
        }

        if ($(resultado_cultivo).val().trim().length > 0 && $(fecha_cultivo).val().trim().length > 0) {
            parametros.push({
                CultivoId: id,
                Resultado: $(resultado_cultivo).val(),
                Fecha: $(fecha_cultivo).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#CultivosJSON").val('');
    }

    $("#CultivosJSON").val(JSON.stringify(parametros));

    return isValid;
}

function GetSensibilidadJSON() {
    let parametros = [];
    
    $("#tblCultivos tbody tr .list-sensibilidad li").each(function () {
        let id = $(this).data("cultivo-id");
        let valor = $(this).data("valor");

        parametros.push({
            CultivoId: id,
            Sensibilidad: valor
        });
    });
    

    if (parametros.length === 0) {
        return '';
    }

    return JSON.stringify(parametros);
}

function GetHisopadoJSON() {
    let parametros = [];
    let isValid = true;

    $("#tblHisopado tbody tr").each(function () {
        let id = $(this).data("hisopado-id");
        let resultado_hisopado = $(this).find("select[name='resultado_hisopado']");
        let fecha_hisopado = $(this).find("input[name='fecha_hisopado']");

        $(fecha_hisopado).closest("td").removeClass("has-error");
        $(resultado_hisopado).closest("td").removeClass("has-error");

        if ($(resultado_hisopado).val().trim().length > 0) {
            if ($(fecha_hisopado).val().trim().length === 0) {
                $(fecha_hisopado).closest("td").addClass("has-error");
                isValid = false;
            }
        }

        if ($(fecha_hisopado).val().trim().length > 0) {
            if ($(resultado_hisopado).val().trim().length === 0) {
                $(resultado_hisopado).closest("td").addClass("has-error");
                isValid = false;
            }
        }

        if ($(resultado_hisopado).val().trim().length > 0 && $(fecha_hisopado).val().trim().length > 0) {
            parametros.push({
                HisopadoId: id,
                Resultado: $(resultado_hisopado).val(),
                Fecha: $(fecha_hisopado).val()
            });
        }
    });

    if (parametros.length === 0) {
        $("#HisopadoJSON").val('');
    }

    $("#HisopadoJSON").val(JSON.stringify(parametros));

    return isValid;
}

function ValidarFormInfectologico() {
    let isValid = true;

    
    $("#modalInfectologico input[name='EstadoInfecto']").closest(".form-group").find("em").remove();

    if ($("#modalInfectologico input[name='EstadoInfecto']:checked").length === 0) {
        $("#modalInfectologico input[name='EstadoInfecto']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar el Estado infectológico</em>`);
        isValid = false;
    }

    return isValid;
}

function GuardarFormularioInfectologico(button) {
    let cultivos_valid = GetCultivosJSON();
    let hisopado_valid = GetHisopadoJSON();

    if (!ValidarFormInfectologico() || !cultivos_valid || !hisopado_valid) {
        return false;
    }

    let cobertura_atb = GetCoberturaAtbJSON();
    
    let sensibilidad = GetSensibilidadJSON();
    

    let form_json = {
        PacienteId: $("#Id").val(),
        EstadoInfectologicoId: $("#modalInfectologico input[name='EstadoInfecto']:checked").val(),
        CoberturaAtbParamJSON: cobertura_atb,
        CultivoParamJSON: $("#modalInfectologico #CultivosJSON").val(),
        SensibilidadParamJSON: sensibilidad,
        HisopadoParamJSON: $("#modalInfectologico #HisopadoJSON").val(),
        Interconsulta: $("#modalInfectologico #Interconsulta").val(),
        Eventos: $("#modalInfectologico #Eventos").val(),
        Planes: $("#modalInfectologico #Planes").val()
    };

    $(button).button('loading');

    $.ajax({
        method: 'POST',
        url: '/Evolucion/AddInfectologico',
        data: form_json
    })
        .done(function (data) {
            if (data.Success) {
                $("#modalInfectologico").modal("toggle");
                LoadInfectologico();
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $(button).button('reset');
        });

}