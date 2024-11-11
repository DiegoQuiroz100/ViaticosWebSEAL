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
            var listaVehiculos = vehiculos.ObtenerHistorialVehiculo(item);

            // Pasar la lista de vehículos a la vista
            return View(listaVehiculos);
        }


        public ActionResult ValeCombustible()
        {
           
            return View();
        }
    }
}