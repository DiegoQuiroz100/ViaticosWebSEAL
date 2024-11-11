using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class VehFacturacion
    {
        public int Item { get; set; }
        public int? IDGrupo { get; set; }
        public DateTime? FecReg { get; set; }
        public int? UsuCrea { get; set; }
        public int? Codgrifo { get; set; }
        public DateTime? FecFac { get; set; }
        public string NroDoc { get; set; }
        public int? NroSol { get; set; }
        public decimal? Monto { get; set; }
        public decimal? IGV { get; set; }
        public decimal? Total { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public string RUC { get; set; }
        public string Periodo { get; set; }
    }
}
