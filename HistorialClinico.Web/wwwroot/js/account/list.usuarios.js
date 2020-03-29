$(function () {
    SetearGrid();

    $("#btnDlgUsuario").click(function () {
        $("#modalAddUsuario input").val('');
        $("#modalAddUsuario").modal("show");
    });

    SetearValidacionDeCampos();

    $("#btnAddUsuario").click(function () {
        AgregarUsuario(this);
        return false;
    });
});

function SetearGrid() {
    $("#divListado").kendoGrid({
        type: "json",
        dataSource: {
            error: function (e) {
                var msg = e.xhr.responseText;
                $("#divListado").data('kendoGrid').cancelChanges();
                alert(msg);
            },
            transport: {
                read: {
                    url: "/Account/_List",
                    type: "POST",
                    dataType: "json"
                }
            },
            requestEnd: function (e) {
                var response = e.response;

                if (!response.Success) {
                    $("#divListado").data('kendoGrid').cancelChanges();
                    alert(response.ErrorMessage);
                }

            },
            pageSize: 20,
            schema: {
                data: "Data",
                total: "Total",
                errors: "Errors"
            }
        },
        height: 550,
        groupable: false,
        sortable: true,
        pageable: false,
        columns: [
            {
                field: "UserName",
                title: "Usuario",
                media: "(min-width: 450px)"
            },
            {
                field: "Email",
                title: "Email",
                media: "(min-width: 450px)"
            },
            {
                title: "Usuario",
                template: kendo.template($("#responsive-column-template").html()),
                media: "(max-width: 450px)"
            }]
    });
}

function AgregarUsuario(button) {
    if (!ValidarFormulario("#formAddUsuario")) {
        return false;
    }

    $(button).button('loading');

    $.ajax({
        method: 'POST',
        url: '/Account/AddUsuario',
        data: $("#formAddUsuario").serialize()
    })
        .done(function (data) {
            if (data.Success) {
                MostarMensaje("Datos guardados correctamente");

                $("#divListado").data().kendoGrid.dataSource.read();

                $("#modalAddUsuario").modal("toggle");
            }
            else {
                MostarError(data.ErrorMessage);
            }

            $(button).button('reset');
        });
}