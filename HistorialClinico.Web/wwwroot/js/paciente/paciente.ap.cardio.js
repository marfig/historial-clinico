function LoadApCardiovascular() {
    let paciente_id = $("#Id").val();
    $("#divApCardiovascular").load(`/Evolucion/_ApCardiovascular?PacienteId=${paciente_id}`, function () { SetNumricAndDates(); });
}

function NuevoApCardio(ApCardioId) {
    $("#modalApCardio").modal("show");
}

function CargarInotropicos(ApCardioId) {
    $("#modalApCardio #ddlInotropicos").empty();
    $("#modalApCardio #ddlInotropicos").append(new Option("-- Seleccionar Inotrópico", ""));

    $.ajax({
        method: 'POST',
        url: `/Evolucion/ListInotropicos?ApCardiovascularId=${ApCardioId}`
    })
        .done(function (data) {
            if (data.Success) {
                for (let i = 0; i < data.Listado.length; i++) {
                    $("#modalApCardio #ddlInotropicos").append(new Option(data.Listado[i].Text, data.Listado[i].Value));
                }
            }
        })
}

function CargarEnzimasCardiacas(ApCardioId) {
    $("#modalApCardio #tblEnzimas tbody").empty();

    $.ajax({
        method: 'POST',
        url: `/Evolucion/ListEnzimasCardiacas?ApCardiovascularId=${ApCardioId}`
    })
        .done(function (data) {
            if (data.Success) {
                for (let i = 0; i < data.Listado.length; i++) {
                    let fila = `<tr data-id="${data.Listado[i].Value}">
									<td>${data.Listado[i].Text}</td>
									<td class="col-md-3"><input name="valor_enzima" class="form-control numeric" /></td>
									<td class="col-md-3">
										<select class="form-control">
											<option value="">-- Curva</option>
											<option value="N">Normal</option>
											<option value="A">Aumentado</option>
											<option value="D">Disminuido</option>
										</select>
									</td>
								</tr>`;
                    $("#modalApCardio #tblEnzimas tbody").append(fila);
                }

                SetNumricAndDates();
            }
        });
}

function AddInotropico() {
    let isValid = true;
    let id = 0;
    let nombre = '';
    let valor = 0;

    if ($("#modalApCardio #ddlInotropicos").val().length === 0) {
        $("#modalApCardio #ddlInotropicos").closest("td").addClass("has-error");
        $("#modalApCardio #ddlInotropicos").closest("td").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalApCardio #ddlInotropicos").closest("td").removeClass("has-error");
        $("#modalApCardio #ddlInotropicos").closest("td").addClass("has-success");
        id = $(ddlInotropicos).val(); 
        nombre = $("#modalApCardio #ddlInotropicos option:selected").text();
    }

    if ($("#modalApCardio #txtValorInotropico").val().length === 0) {
        $("#modalApCardio #txtValorInotropico").closest("td").addClass("has-error");
        $("#modalApCardio #txtValorInotropico").closest("td").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalApCardio #txtValorInotropico").closest("td").removeClass("has-error");
        $("#modalApCardio #txtValorInotropico").closest("td").addClass("has-success");
        valor = $("#modalApCardio #txtValorInotropico").val();
    }

    if (isValid) {
        let fila = `<tr data-id="${id}" data-valor="${valor}">
						<td><span>${nombre}</span></td>
						<td><span>${valor}</span></td>
						<td class="col-md-1 text-center" style="vertical-align: middle">
							<a href="javascript:void('0')" onclick="RemoveInotropico(this)"><i class="glyphicon glyphicon-remove"></i></a>
						</td>
					</tr>`;

        $("#tblInotropicos tbody").append(fila);

        $("#modalApCardio #ddlInotropicos").val('');
        $("#modalApCardio #txtValorInotropico").val('');
        $(`#modalApCardio #ddlInotropicos option[value='${id}']`).remove();
    }
}

function RemoveInotropico(item) {
    let fila = $(item).closest("tr");
    let id = $(fila).data("id");
    let nombre = $(fila).data("nombre");

    $("#modalApCardio #ddlInotropicos").append(new Option(nombre, id));

    $(fila).remove();
}

function GuardarFormularioApCardiovascular(button) {
    if (!ValidarFormApCardiovascular()) {
        return false;
    }

    let inotropicos = GetInotropicosJSON();
    let enzimas = GetEnzimasJSON();
    
    let form_json = {
        PacienteId: $("#Id").val(),
        Estado: $("#modalApCardio input[name='Estado']:checked").val(),
        EvaluacionCardiologica: $("#modalApCardio #EvaluacionCardiologica").val(),
        InotropicosJSON: inotropicos,
        EnzimasCardiacasJSON: enzimas,
        Eventos: $("#modalApCardio #Eventos").val(),
        Planes: $("#modalApCardio #Planes").val()
    };

    $(button).button('loading');

    $.ajax({
        method: 'POST',
        url: '/Evolucion/AddApCardiovascular',
        data: form_json
    })
        .done(function (data) {
            if (data.Success) {
                $("#modalApCardio").modal("toggle");
                LoadApCardiovascular();
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $(button).button('reset');
        });
}

function ValidarFormApCardiovascular() {
    let isValid = true;

    if ($("#modalApCardio #EvaluacionCardiologica").val().length === 0) {
        $("#modalApCardio #EvaluacionCardiologica").closest(".form-group").addClass("has-error");
        $("#modalApCardio #EvaluacionCardiologica").closest(".form-group").removeClass("has-success");
        isValid = false;
    }
    else {
        $("#modalApCardio #EvaluacionCardiologica").closest(".form-group").removeClass("has-error");
        $("#modalApCardio #EvaluacionCardiologica").closest(".form-group").addClass("has-success");
    }

    $("#modalApCardio input[name='Estado']").closest(".form-group").find("em").remove();

    if ($("#modalApCardio input[name='Estado']:checked").length === 0) {
        $("#modalApCardio input[name='Estado']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar un Estado</em>`);
        isValid = false;
    }

    return isValid;
}

function GetInotropicosJSON() {
    let inotropicos = [];

    $("#tblInotropicos tbody tr").each(function () {
        let id = $(this).data("id");
        let valor = $(this).data("valor");
        inotropicos.push({ Id: id, Valor: valor });
    });

    if (inotropicos.length === 0) {
        return '';
    }

    return JSON.stringify(inotropicos);
}

function GetEnzimasJSON() {
    let enzimas = [];

    $("#tblEnzimas tbody tr").each(function () {
        let id = $(this).data("id");
        let valor = $(this).find("input[name='valor_enzima']").val();
        let curva = $(this).find("select[name='curva']").val();

        if (valor.trim().length > 0) {
            enzimas.push({ Id: id, Valor: valor, Curva: curva });
        }
    });

    if (enzimas.length === 0) {
        return '';
    }

    return JSON.stringify(enzimas);

    return enzimas;
}