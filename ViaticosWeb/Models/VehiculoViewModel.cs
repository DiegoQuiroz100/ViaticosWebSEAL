using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class VehiculoViewModel
    {
        public string Marca { get; set; }
        public string Anio { get; set; }
        public string Tipo { get; set; }
        public string AreaAsignada { get; set; }
        public string Asignado { get; set; }
        public string Estado { get; set; }
        public string Combustible { get; set; }
        public string CodAct { get; set; }
    }
}