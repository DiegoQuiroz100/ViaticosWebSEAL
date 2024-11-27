using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class ListaDetalle
    {
        public int SolCod { get; set; }         // Código de la solicitud
        public string Descripcion { get; set; } // Descripción del detalle
        public decimal Cantidad { get; set; }   // Cantidad asociada al detalle
    }
}