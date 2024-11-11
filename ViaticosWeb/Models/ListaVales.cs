using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViaticosWeb.Models
{
    public class ListaVales
    {
        public int SolCod { get; set; }               // Código de la solicitud
        public string MarNom { get; set; }            // Nombre de la marca
        public string Vehpla { get; set; }            // Placa del vehículo
        public string Asignado { get; set; }          // Persona o entidad asignada
        public string Descripcion { get; set; }       // Descripción (ej. "VALES DE COMBUSTIBLE")
        public decimal Km { get; set; }               // Kilómetros
        public decimal Cantidad { get; set; }         // Cantidad de combustible
        public string Grifo { get; set; }             // Nombre del grifo
        public string Vo2cod { get; set; }            // Código VO2
        public string TipMed { get; set; }            // Tipo de medición (ej. "Galones")
        public string Estado { get; set; }            // Estado (ej. "Facturado")
        public DateTime FecReg { get; set; }          // Fecha de registro
        public decimal Pu { get; set; }               // Precio unitario
        public decimal Total { get; set; }            // Total
        public string NroDoc { get; set; }            // Número de documento
        public string Ruc { get; set; }               // RUC de la entidad
        public string AreNom { get; set; }            // Nombre de la área o gerencia
    }
}