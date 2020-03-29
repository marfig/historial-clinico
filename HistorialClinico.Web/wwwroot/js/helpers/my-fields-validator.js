function SetearValidacionDeCampos() {
    $(".validate").change(function () {
        ValidarCampo(this);
    });
}

function ValidarFormulario(form) {
    var isValid = true;

    $(`${form} .validate`).each(function () {
        $(this).parents(".form-group").find("em").remove();
        $(this).parents(".form-group").removeClass("has-error").removeClass("has-success");

        var message = $(this).data("validation-message");

        if ($(this).val().length === 0) {
            $(this).parents(".form-group").addClass("has-error").removeClass("has-success");
            $(this).closest(".form-group").append(`<em>${message === undefined ? "" : message}</em>`);
            isValid = false;
        }
        else {
            if ($(this).attr("type") === "email" && !validateEmail($(this).val())) {
                $(this).parents(".form-group").addClass("has-error").removeClass("has-success");
                $(this).after(`<em>Debe ingresar un email válido</em>`);
                isValid = false;
            }
            else {
                $(this).parents(".form-group").removeClass("has-error").addClass("has-success");
            }
        }
    });

    return isValid;
}

function ValidarCampo(item) {
    $(item).closest(".form-group").find("em").remove();
    $(item).parents(".form-group").removeClass("has-error").removeClass("has-success");

    if ($(item).val().length === 0) {
        $(item).parents(".form-group").addClass("has-error").removeClass("has-success");
    }
    else {
        if ($(item).attr("type") === "email" && !validateEmail($(item).val())) {
            $(item).parents(".form-group").addClass("has-error").removeClass("has-success");
        }
        else {
            $(item).parents(".form-group").removeClass("has-error").addClass("has-success");
        }
    }
}

function validateEmail(Email) {
    var pattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;

    return $.trim(Email).match(pattern) ? true : false;
}

function ValidarCamposTabla(filas_validar) {
    var isValid = true;

    var fields = $(filas_validar).find('.table-field-validate');

    $(fields).each(function () {
        $(this).closest("td").removeClass("has-error").removeClass("has-success");

        if ($(this).val().length === 0) {
            $(this).closest("td").addClass("has-error").removeClass("has-success");
            isValid = false;
        }
        else {
            $(this).closest("td").removeClass("has-error").addClass("has-success");
        }
    });

    return isValid;
}