using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistorialClinico.Web.Utils
{
    public static class FuncionesUtiles
    {
        public static string CalcularEdad(DateTime? fecha_nacimiento)
        {
            if (!fecha_nacimiento.HasValue)
            {
                return "";
            }

            var dias = (DateTime.Now - fecha_nacimiento.Value).Days;

            if (dias > 365)
            {
                var anhos = dias / 365;
                dias -= (365 * anhos);

                if (dias > 30)
                {
                    var meses = dias / 30;
                    dias -= (30 * meses);

                    if (dias == 0)
                    {
                        return $"{anhos} años, {meses} meses";
                    }

                    return $"{anhos} años, {meses} meses, {dias} días";
                }

                return $"{anhos} años, {dias} días";
            }

            if (dias > 30)
            {
                var meses = dias / 30;
                dias -= (30 * meses);

                if (dias == 0)
                {
                    return $"{meses} meses";
                }

                return $"{meses} meses, {dias} días";
            }

            return $"{dias} días";
        }
    }
}
