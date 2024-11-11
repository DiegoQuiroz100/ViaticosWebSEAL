using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    class VehDet
    {
        public int Item { get; set; }
        public int? Solcod { get; set; }
        public int? Codmot { get; set; }
        public string Descripcion { get; set; }
        public decimal? Cantidad { get; set; }
        public DateTime? Fecreg { get; set; }
        public string Anno { get; set; }
        public string TipMed { get; set; }
        public decimal? Pu { get; set; }
        public decimal? Total { get; set; }
        public decimal? Cantidadmod { get; set; }
    }
}
