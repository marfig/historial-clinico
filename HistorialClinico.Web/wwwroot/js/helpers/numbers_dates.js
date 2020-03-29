$(function () {
    SetNumricAndDates();
});

function SetNumricAndDates() {
    $('.numeric').on('change click keyup input paste', (function (event) {
        $(this).val(function (index, value) {
            if (event.which === 110) {
                value = value + ',';
            }

            return value.replace(/(?!\,)\D/g, "").replace(/(?<=\,,*)\,/g, "").replace(/(?<=\,\d\d\d\d\d\d).*/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".");
        });
    }));

    $('.date').datepicker({
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        prevText: '<i class="fa fa-chevron-left"></i>',
        nextText: '<i class="fa fa-chevron-right"></i>'
    });
}

function Numeric(valor) {
    if (typeof valor === "string") {
        valor = valor.replace(/\./g, '');
        valor = valor.replace(',','.');
        if (valor !== '') {
            return parseFloat(valor);
        }
        else {
            return 0;
        }
    } else
        return valor;
}

function formatNumbers(valor) {
    if (isNaN(valor)) {
        valor = '';
    }
    else {
        valor = Math.round(valor * 100) / 100;
        valor = valor.toString().replace('.', ',');
        valor = addCommas(valor);
    }
    return valor;
}

function restaFechas(f1, f2) {
    var aFecha1 = f1.split('/');
    var aFecha2 = f2.split('/');
    var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
    var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
    var dif = fFecha1 - fFecha2;
    var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
    return dias;
}

function fechaSumarDias(dateStr, days) {
    var date = toDate(dateStr);
    var newdate = new Date(date);

    newdate.setDate(newdate.getDate() + days);

    var dd = newdate.getDate();
    var mm = newdate.getMonth() + 1;
    var y = newdate.getFullYear();

    return padLeft(dd) + '/' + padLeft(mm) + '/' + y;
}

function toDate(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function padLeft(value) {
    if (value.toString().length === 1) {
        return '0' + value;
    }

    return value;
}