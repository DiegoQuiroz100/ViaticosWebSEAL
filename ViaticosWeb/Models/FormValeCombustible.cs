using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class FormValeCombustible
    {
    
        public int Id { get; set; }

        // Información de la Gerencia y Unidad
       
        public string Gerencia { get; set; }

        
        public string Unidad { get; set; }

        // Información del Vehículo
        
        public string PlacaRodaje { get; set; }

       
        public string Marca { get; set; }

       
        public int Anio { get; set; }

        public string Tipo { get; set; }

        
        public string Kilometraje { get; set; }

        public string AreaAsignada { get; set; }

       
        public string Asignado { get; set; }

        
        public string Estado { get; set; }

      
        public string Combustible { get; set; }

        // Información del Grifo
       
        public string Grifo { get; set; }

      
        public int CantidadGalones { get; set; }
    }
}