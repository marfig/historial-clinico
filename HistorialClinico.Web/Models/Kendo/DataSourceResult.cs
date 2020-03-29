using System.Collections;

namespace HistorialClinico.Web.Models.Kendo
{
    public class DataSourceResult
    {
        public bool Success = true;
        public IEnumerable Data { get; set; }

        public object Errors { get; set; }

        public int Total { get; set; }

        public string MontoTotal { get; set; }
    }
}
