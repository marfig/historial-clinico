$(function () {
    SetearGrid();
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
                    url: "/Paciente/Listado",
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
                field: "Cama",
                title: "Cama",
                width: "80px",
                media: "(min-width: 450px)"
            },
            {
                field: "Nombre",
                title: "Nombre",
                media: "(min-width: 450px)"
            },
            {
                field: "Sexo",
                title: "Sexo",
                media: "(min-width: 450px)"
            },
            {
                field: "Edad",
                title: "Edad",
                media: "(min-width: 450px)"
            },
            {
                title: "Paciente",
                template: kendo.template($("#responsive-column-template").html()),
                media: "(max-width: 450px)"
            }]
    });
}