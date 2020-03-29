function LoadCirugia() {
    let paciente_id = $("#Id").val();

    $("#divCirugia").load(`/Evolucion/_Cirugia?PacienteId=${paciente_id}`, function () { SetEventosCirugia(); });
}

function SetEventosCirugia() {
    SetNumricAndDates();

}

function NuevoCirugia() {
    $("#modalCirugia").modal("show");
}


function GuardarFormularioCirugia(button) {

    if (!ValidarFormCirugia()) {
        return false;
    }

    let tuvo_cirugia = $("#modalCirugia input[name='TuvoCirugia']:checked").val() === "SI"; 
    let procedimiento = $("#modalCirugia input[name='Procedimiento']:checked").val() === "SI"; 
    
    let form_json = {
        PacienteId: $("#Id").val(),
        TuvoCirugia: tuvo_cirugia,
        Tecnica: $("#modalCirugia input[name='Tecnica']").val(),
        Hallazgos: $("#modalCirugia input[name='Hallazgos']").val(),
        Procedimiento: procedimiento,
        CualProcedimiento: $("#modalCirugia input[name='CualProcedimiento']").val(),
        OtrasAcotaciones: $("#modalCirugia textarea[name='OtrasAcotaciones']").val()
    };

    $(button).button('loading');

    $.ajax({
        method: 'POST',
        url: '/Evolucion/AddCirugia',
        data: form_json
    })
        .done(function (data) {
            if (data.Success) {
                $("#modalCirugia").modal("toggle");
                LoadCirugia();
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $(button).button('reset');
        });
}

function ValidarFormCirugia() {
    let isValid = true;

    $("#modalCirugia input[name='TuvoCirugia']").closest(".form-group").find("em").remove();

    if ($("#modalCirugia input[name='TuvoCirugia']:checked").length === 0) {
        $("#modalCirugia input[name='TuvoCirugia']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar una opción</em>`);
        isValid = false;
    }

    if ($("#modalCirugia input[name='Procedimiento']:checked").length === 0) {
        $("#modalCirugia input[name='Procedimiento']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar una opción</em>`);
        isValid = false;
    }

    return isValid;
}
