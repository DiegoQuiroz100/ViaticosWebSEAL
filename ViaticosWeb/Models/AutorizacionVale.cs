using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class AutorizacionVale
    {
        // Representa el código de solicitud (solcod)
        public int Solcod { get; set; }

        // Marca del vehículo
        public string Marca { get; set; }

        // Placa del vehículo
        public string Placa { get; set; }

        // Persona asignada al vehículo o uso asignado
        public string Asignado { get; set; }

        // Descripción de la solicitud o tipo de mantenimiento
        public string Descripcion { get; set; }

        // Kilometraje del vehículo
        public int KM { get; set; }

        // Cantidad de combustible solicitada
        public int Cantidad { get; set; }

        // Unidad de medida del combustible (e.g., litros, galones)
        public string Unidad { get; set; }

        // Nombre del grifo o estación de servicio
        public string Grifo { get; set; }

        // Para permitir la selección en la vista (si deseas mostrar como checkboxes en la vista)
        public bool Seleccionado { get; set; } = false;
    }
}