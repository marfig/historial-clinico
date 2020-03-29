$(function () {
    let paciente_id = $("#PacienteId").val();

    LoadCardio(paciente_id);
    LoadApResp(paciente_id);
    LoadInfecto(paciente_id);
    LoadHMN(paciente_id);
    LoadSNC(paciente_id);
    LoadHemato(paciente_id);
    LoadCirugia(paciente_id);
});

function LoadCardio(paciente_id) {
    $("#divApCardiovascular").load(`/ResumenPaciente/_ApCardiovascular?PacienteId=${paciente_id}`);
}

function LoadApResp(paciente_id) {
    $("#divApRespiratorio").load(`/ResumenPaciente/_ApRespiratorio?PacienteId=${paciente_id}`);
}

function LoadInfecto(paciente_id) {
    $("#divInfectologico").load(`/ResumenPaciente/_Infecto?PacienteId=${paciente_id}`);
}

function LoadHMN(paciente_id) {
    $("#divHMN").load(`/ResumenPaciente/_HMN?PacienteId=${paciente_id}`);
}

function LoadSNC(paciente_id) {
    $("#divSNC").load(`/ResumenPaciente/_SNC?PacienteId=${paciente_id}`);
}

function LoadHemato(paciente_id) {
    $("#divHematologico").load(`/ResumenPaciente/_Hematologico?PacienteId=${paciente_id}`);
}

function LoadCirugia(paciente_id) {
    $("#divCirugia").load(`/ResumenPaciente/_Cirugia?PacienteId=${paciente_id}`);
}