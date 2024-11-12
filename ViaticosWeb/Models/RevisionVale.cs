using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class RevisionVale
    {
        public int Item { get; set; }
        public string Solicitud { get; set; }
        public string Marca { get; set; }
        public string Placa { get; set; }
        public string Asignado { get; set; }
        public string Descripcion { get; set; }
        public int KM { get; set; }
        public int Cantidad { get; set; }
        public string Unidad { get; set; }
        public string Grifo { get; set; }
    }
}