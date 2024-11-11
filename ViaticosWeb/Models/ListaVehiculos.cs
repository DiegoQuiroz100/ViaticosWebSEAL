using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class ListaVehiculos
    {
        //lista vehiculos
        public int Item { get; set; }
        public string CodAct { get; set; }
        public string VehPla { get; set; }
        public string MarNom { get; set; }
        public string Modelo { get; set; }
        public string Combustible { get; set; }
        public int VehKm { get; set; }
        public string VehEst { get; set; }
        public int VehAno { get; set; }
        public string AreaNom { get; set; }
        public string Asignado { get; set; }


        //para vehcbab
        public int SolCod { get; set; }
        public string Tipo { get; set; } // Mantenimiento o Combustible
        public DateTime FechaRegistro { get; set; }
        public int KmAnt { get; set; }
        public int KmActual { get; set; }
        public int Km { get; set; }
        public string Motivo { get; set; }
        public string Observaciones { get; set; }
        public string Grifo { get; set; }

        //vehdet
      
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
    }
}