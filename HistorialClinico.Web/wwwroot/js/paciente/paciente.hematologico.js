function LoadHematologico() {
    let paciente_id = $("#Id").val();

    $("#divHematologico").load(`/Evolucion/_Hematologico?PacienteId=${paciente_id}`, function () { SetEventosHematologico(); });
}

function SetEventosHematologico() {
    SetNumricAndDates();

}

function NuevoHematologico() {
    $("#modalHematologico").modal("show");
}


function GuardarFormularioHematologico(button) {

    if (!ValidarFormHematologico()) {
        return false;
    }

    let vitaminak = $("#modalHematologico input[name='VitaminaK']:checked").val() === "SI"; 
    let sangrado = $("#modalHematologico input[name='SangradoActivo']:checked").val() === "SI"; 
    let transfusion = $("#modalHematologico input[name='Transfusiones']:checked").val() === "SI"; 
    
    let form_json = {
        PacienteId: $("#Id").val(),
        Hemograma_PAI: $("#modalHematologico input[name='Hemograma_PAI']").val(),
        Hemograma_HB: $("#modalHematologico input[name='Hemograma_HB']").val(),
        Hemograma_HTC: $("#modalHematologico input[name='Hemograma_HTC']").val(),
        Hemograma_PLT: $("#modalHematologico input[name='Hemograma_PLT']").val(),
        Crasis_TP: $("#modalHematologico input[name='Crasis_TP']").val(),
        Crasis_TTPA: $("#modalHematologico input[name='Crasis_TTPA']").val(),
        Crasis_Fibrinoginos: $("#modalHematologico input[name='Crasis_Fibrinoginos']").val(),
        VitaminaK: vitaminak,
        DosisVitaminaK: $("#modalHematologico input[name='DosisVitaminaK']").val(),
        FechaVitaminaK: $("#modalHematologico input[name='FechaVitaminaK']").val(),
        SangradoActivo: sangrado,
        LugarSangrado: $("#modalHematologico textarea[name='LugarSangrado']").val(),
        Transfusiones: transfusion,
        Transfusiones_GRC: $("#modalHematologico input[name='Transfusiones_GRC']").val(),
        Transfusiones_PFC: $("#modalHematologico input[name='Transfusiones_PFC']").val(),
        Transfusiones_CRIO: $("#modalHematologico input[name='Transfusiones_CRIO']").val(),
        Transfusiones_PLT: $("#modalHematologico input[name='Transfusiones_PLT']").val(),
        Eventos: $("#modalHematologico #Eventos").val(),
        Planes: $("#modalHematologico #Planes").val()
    };

    $(button).button('loading');

    $.ajax({
        method: 'POST',
        url: '/Evolucion/AddHematologico',
        data: form_json
    })
        .done(function (data) {
            if (data.Success) {
                $("#modalHematologico").modal("toggle");
                LoadHematologico();
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $(button).button('reset');
        });
}

function ValidarFormHematologico() {
    let isValid = true;

    $("#modalHematologico input[name='VitaminaK']").closest(".form-group").find("em").remove();
    let vitamina_k_length = $("#modalHematologico input[name='VitaminaK']:checked").length;

    if (vitamina_k_length === 0) {
        $("#modalHematologico input[name='VitaminaK']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar una opción</em>`);
        isValid = false;
    }

    $("#modalHematologico input[name='DosisVitaminaK']").closest(".form-group").find("em").remove();
    $("#modalHematologico input[name='FechaVitaminaK']").closest(".form-group").find("em").remove();

    if (vitamina_k_length > 0) {
        if ($("#modalHematologico input[name='DosisVitaminaK']").val().trim().length === 0) {
            $("#modalHematologico input[name='DosisVitaminaK']").closest(".form-group").append(`<em style="color: #b94a48">Debe ingresar la dosis</em>`);
            isValid = false;
        }

        if ($("#modalHematologico input[name='FechaVitaminaK']").val().trim().length === 0) {
            $("#modalHematologico input[name='FechaVitaminaK']").closest(".form-group").append(`<em style="color: #b94a48">Debe ingresar la fecha</em>`);
            isValid = false;
        }
    }

    $("#modalHematologico input[name='SangradoActivo']").closest(".form-group").find("em").remove();

    if ($("#modalHematologico input[name='SangradoActivo']:checked").length === 0) {
        $("#modalHematologico input[name='SangradoActivo']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar una opción</em>`);
        isValid = false;
    }

    $("#modalHematologico textarea[name='LugarSangrado']").closest(".form-group").find("em").remove();

    if ($("#modalHematologico textarea[name='LugarSangrado']").val().trim().length === 0) {
        $("#modalHematologico textarea[name='LugarSangrado']").closest(".form-group").append(`<em style="color: #b94a48">Debe ingresar el lugar del sangrado</em>`);
        isValid = false;
    }

    $("#modalHematologico input[name='Transfusiones']").closest(".form-group").find("em").remove();

    if ($("#modalHematologico input[name='Transfusiones']:checked").length === 0) {
        $("#modalHematologico input[name='Transfusiones']").closest(".form-group").append(`<em style="color: #b94a48">Debe seleccionar una opción</em>`);
        isValid = false;
    }

    return isValid;
}
