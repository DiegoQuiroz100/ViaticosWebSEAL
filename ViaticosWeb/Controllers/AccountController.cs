using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViaticosWeb.Models;

namespace ViaticosWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService userService;

        public AccountController()
        {
            // Reemplaza con tu cadena de conexión
            string connectionString = "Data Source=192.168.53.43;Initial Catalog=AdmUsu;User ID=sa;Password=S3@lit02024$7;";
            userService = new UserService(connectionString);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Usuario y contraseña son obligatorios.";
                return View();
            }

            int userId = userService.ValidateUser(username, password);

            if (userId > 0)
            {
                // Autenticación exitosa
                Session["UserId"] = userId;
                Session["Permisos"] = ("1,3,4,5,6,67,68,69,91,101,102,107,109");
                return RedirectToAction("Index", "Viaticos");
            }
            else
            {
                // Falló la autenticación
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            // Limpia la sesión
            Session.Clear();
            Session.Abandon();

            // Redirige al inicio de sesión
            return RedirectToAction("Login", "Account");
        }
    }
}
