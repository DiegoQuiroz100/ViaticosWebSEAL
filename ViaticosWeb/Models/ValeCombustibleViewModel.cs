using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class ValeCombustibleViewModel
    {
        public int PlacaId { get; set; }
        public string Placa { get; set; }
        public double Kilometraje { get; set; }
        public double Cantidad { get; set; }
        public int GrifoId { get; set; }
        public int AreaId { get; set; }
    }
}