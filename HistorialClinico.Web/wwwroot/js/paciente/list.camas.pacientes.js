$(function () {
    CargarListado();
});

function CargarListado() {
     $("#divListado").load("/CamaPaciente/_Listado", function () {
        AutocompletePaciente($("table tbody tr td input[name='paciente']"));
    });
     
}

function AutocompletePaciente(elemento) {
    $(elemento).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Paciente/BuscarPaciente",
                type: "POST",
                dataType: "json",
                data: { valor: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: `${item.Nombres} ${item.Apellidos} (Doc. Nro.: ${item.NroDocumento})`,
                            value: `${item.Nombres} ${item.Apellidos} (Doc. Nro.: ${item.NroDocumento})`,
                            id: item.Id
                        };
                    }))
                }
            })
        },
        change: function (event, ui) {
            if (!ui.item) {
                $(this).val("");
                $(this).data("paciente-id", "");
            }
        },
        select: function (event, ui) {
            $(this).val(ui.item.label);
            $(this).data("paciente-id", ui.item.id);
        }
    });
}

function AgregarPaciente(item) {
    let fila = $(item).closest("tr");

    let input = $(fila).find("input[name='paciente']");

    let paciente = $(input).val();

    if (paciente.trim().length === 0) {
        window.location = $(fila).find("a name=['lnk_agregar']").href;
        return false;
    }

    let paciente_id = $(input).data("paciente-id");
    let cama_id = $(fila).data("cama-id");

    $.ajax({
        method: 'POST',
        url: '/CamaPaciente/AsociarCamaPaciente',
        data: { PacienteId: paciente_id, CamaId: cama_id }
    })
        .done(function (data) {
            if (data.Success) {
                window.location.reload();
            }
            else {
                MostarError(data.ErrorMessage);
            }
        });
}

function ConfirmEliminarPaciente(item) {
    $.SmartMessageBox({
        title: 'Eliminar Paciente',
        content: "¿Confirma que desea eliminar?",
        buttons: '[No][Si]'
    }, function (ButtonPressed) {
        if (ButtonPressed === "Si") {
            EliminarPaciente(item);
        }
    });
}

function EliminarPaciente(item) {
    let fila = $(item).closest("tr");

    let paciente_id = $(fila).data("paciente-id");
    let cama_id = $(fila).data("cama-id");

    $.ajax({
        method: 'POST',
        url: '/CamaPaciente/DesasociarCamaPaciente',
        data: { PacienteId: paciente_id, CamaId: cama_id }
    })
        .done(function (data) {
            if (data.Success) {
                window.location.reload();
            }
            else {
                MostarError(data.ErrorMessage);
            }
        });
}