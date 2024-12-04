using ViaticosWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.SqlClient;

namespace ViaticosWeb.Controllers
{
   
    public class ViaticosController : Controller
    {
        // GET: Viaticos
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ValeCombustible()
        {
            return View();
        }
        public ActionResult ListaVehiculos (string VehPla)
        {
            dbVehiculos vehiculos = new dbVehiculos();
            var listaVehiculos = vehiculos.ObtenerVehiculos();
            if (!string.IsNullOrEmpty(VehPla))
            {
                listaVehiculos = listaVehiculos.Where(v => v.VehPla.Contains(VehPla)).ToList();
            }

            // Pasar la lista de vehículos a la vista
            return View(listaVehiculos);
        }
        public ActionResult Historial(int item)
        {
            dbVehiculos vehiculos = new dbVehiculos();
            var listaHistorial = vehiculos.ObtenerHistorialVehiculo(item); // Pasa el item
            return View(listaHistorial);
        }

        [HttpGet]
        public JsonResult ObtenerDetalle(int solcod)
        {
            dbVehiculos vehiculos = new dbVehiculos();
            // Llama a tu método para obtener los detalles
            var listaHistorial = vehiculos.ObtenerDetalleHistorial(solcod); // Pasa el item
            // Retorna el resultado como JSON
            return Json(listaHistorial, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListaVales()
        {
            dbVehiculos vales = new dbVehiculos();
            var listavales = vales.ObtenerListaVales();
            

            // Pasar la lista de vehículos a la vista
            return View(listavales);
        }

    }
}