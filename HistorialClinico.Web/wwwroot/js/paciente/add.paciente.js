$(function () {
    SetearValidacionDeCampos();

    $("#btnGuardar").click(function () {
        GuardarDatosBasicos();
        return false;
    });

    $("#btnAddDiagnostico").click(function () {
        AgregarDiagnostico();
        return false;
    });

    SetearGridDiagnosticos();

    $("#bootstrap-wizard-1 .disabled a").click(function () {
        let id = $("#Id").val();
        return id > 0;
    });

    $("#PrmsId").change(function () {
        EditarPRMS();
        return false;
    });

    LoadApCardiovascular();

    LoadApRespiratorio();

    LoadInfectologico();

    LoadSNC();

    LoadHMN();

    LoadHematologico();

    LoadCirugia();
});

function AgregarContacto() {
    $.get("/Paciente/AddContactoPaciente", function (data) {
        $("#divContactos").append(data);
    });
}

function DlgDiagnostico(id, resumen) {
    $("#modalDiagnostico #diagnostico_id").val(id);
    $("#modalDiagnostico #txtResumen").val(resumen);
    $("#modalDiagnostico").modal("show");
}

function GuardarDatosBasicos() {
    if (!ValidarFormulario("#tab1")) {
        return false;
    }

    SerializarContactos();

    $("#btnGuardar").button('loading');

    $.ajax({
        method: 'POST',
        url: '/Paciente/AddDatosBasicos',
        data: $("#formPaciente").serialize()
    })
        .done(function (data) {
            if (data.Success) {
                $("#Id").val(data.PacienteId);
                ActualizarContactos(data.PacienteId);
                MostarMensaje("Datos guardados correctamente");
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $("#btnGuardar").button('reset');
        });
}

function SerializarContactos() {
    let contactos = [];

    $("#divContactos .row").each(function () {
        let id = $(this).data("contacto-id");
        let tipo = $(this).find("select[name='TipoContactoId']").val();
        let nro = $(this).find("input[name='NroContacto']").val();
        let nombre = $(this).find("input[name='NombreContacto']").val();

        contactos.push({ ContactoId: id, TipoContactoId: tipo, NroContacto: nro, NombreContacto: nombre });
    });

    $("#ContactosJSON").val(JSON.stringify(contactos));
}

function ActualizarContactos(paciente_id) {
    $("#divContactos").load(`/Paciente/GetContactosPaciente?PacienteId=${paciente_id}`);
}

function AgregarDiagnostico() {
    if (!ValidarFormulario("#formDiagnostico")) {
        return false;
    }

    let id = $("#Id").val();
    let diagnostico_id = $("#modalDiagnostico #diagnostico_id").val();
    let resumen = $("#modalDiagnostico #txtResumen").val();

    $.ajax({
        method: 'POST',
        url: '/Paciente/AddDiagnostico',
        data: { Id: diagnostico_id, PacienteId: id, Resumen: resumen }
    })
        .done(function (data) {
            if (data.Success) {
                $("#modalDiagnostico").modal("toggle");

                $("#divDiagnosticos").data().kendoGrid.dataSource.read();
            }
            else {
                MostarError(data.ErrorMessage);
            }
        });
}

function SetearGridDiagnosticos() {
    $("#divDiagnosticos").kendoGrid({
        type: "json",
        dataSource: {
            transport: {
                read: {
                    url: "/Paciente/ListDiagnosticos",
                    type: "POST",
                    dataType: "json",
                    data: function () {
                        let paciente_id = $("#Id").val();

                        return {
                            PacienteId: paciente_id
                        };
                    }
                }
            },
            schema: {
                data: "Data",
                total: "Total",
                errors: "Errors"
            }
        },
        groupable: false,
        sortable: false,
        pageable: false,
        columns: [
            {
                field: "Fecha",
                title: "Fecha",
                type: "date",
                format: "{0:dd/MM/yyyy HH:mm}",
                media: "(min-width: 450px)"
            },
            {
                field: "UserName",
                title: "Usuario",
                media: "(min-width: 450px)"
            },
            {
                field: "Resumen",
                title: "Diagnóstico",
                media: "(min-width: 450px)"
            },
            {
                field: "Id",
                title: "Acciones",
                filterable: false,
                sortable: false,
                width: 200,
                attributes: { style: "text-align:center" },
                headerAttributes: { style: "text-align: center" },
                template: `<a class="btn btn-primary" href="javascript:void('0')" onclick="DlgDiagnostico(#=Id#, '#=Resumen#')"><i class="fa fa-pencil"></i> Editar</a> 
                           <a onclick="DlgEliminarDiagnostico(#=Id#); return false;" class="btn btn-danger" href="javascript:void('0')"><i class="fa fa-trash-o"></i> Eliminar</a>`,
                media: "(min-width: 450px)"
            },
            {
                title: "Diagnóstico",
                template: kendo.template($("#responsive-column-template-diagnosticos").html()),
                media: "(max-width: 450px)"
            }]
    });
}

function EditarPRMS() {
    ResetPRMS_input(0);

    let id = $("#Id").val();
    let prms_id = $("#PrmsId").val();

    $.ajax({
        method: 'POST',
        url: '/Paciente/EditPRMS',
        data: { PacienteId: id, PrmsId: prms_id }
    })
        .done(function (data) {
            if (data.Success) {
                $("#checkPRMS").show();
                $("#checkPRMS").closest(".input-group").addClass("has-success");

                ResetPRMS_input(1000);
            }
            else {
                MostarError(data.ErrorMessage);
                $("#PrmsId").val("");
            }
        });
}

function ResetPRMS_input(timeout) {
    setTimeout(function () {
        $("#checkPRMS").hide();
        $("#checkPRMS").closest(".input-group").removeClass("has-success");
    }, timeout);
}

function DlgEliminarDiagnostico(id) {
    $.SmartMessageBox({
        title: "Confirmación!",
        content: "¿Desea eliminar el diagnóstico?",
        buttons: '[No][Sí]'
        }, function (ButtonPressed) {
            if (ButtonPressed === "Sí") {
                EliminarDiagnostico(id);
            }
    });
}

function EliminarDiagnostico(id) {
    $.ajax({
        method: 'POST',
        url: '/Paciente/DeleteDiagnostico',
        data: { DiagnosticoId: id }
    })
        .done(function (data) {
            if (data.Success) {
                $("#divDiagnosticos").data().kendoGrid.dataSource.read();
            }
            else {
                MostarError(data.ErrorMessage);
            }
        });
}

