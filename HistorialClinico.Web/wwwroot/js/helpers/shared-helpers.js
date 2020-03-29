function MostarError(mensaje) {
    $.bigBox({
        title: "Ocurrió un error",
        content: mensaje,
        color: "#C46A69",
        icon: "fa fa-warning shake animated",
        number: "1",
        timeout: 6000
    });
}

function MostarMensaje(mensaje) {
    $.smallBox({
        title: "Notificacion",
        content: mensaje,
        color: "#739E73",
        timeout: 8000,
        icon: "fa fa-check"
    });
}

