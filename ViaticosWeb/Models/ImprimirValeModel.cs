using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class ImprimirValeModel
    {
        public string SolCod { get; set; }       // Código de la solicitud
        public string Marca { get; set; }        // Nombre de la marca
        public string Placa { get; set; }        // Placa del vehículo
        public string Asignado { get; set; }     // Persona a la que está asignado
        public string Descripcion { get; set; }  // Descripción del mantenimiento o actividad
        public int KM { get; set; }              // Kilometraje
        public decimal Cantidad { get; set; }    // Cantidad de combustible o servicio
        public string Grifo { get; set; }        // Grifo donde se carga combustible
        public string CodAct { get; set; }       // Código de actividad
        public string TipoMedida { get; set; }   // Unidad de medida
    }
}