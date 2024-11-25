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
        private readonly UserAuthenticator _authenticator;

        public AccountController()
        {
            string connectionString = "Data Source=192.168.53.43;Initial Catalog=AdmUsu;User ID=sa;Password=S3@lit02024$7;";
            _authenticator = new UserAuthenticator(connectionString);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            int userId = _authenticator.ValidateUser(user.Username, user.Password);

            if (userId > 0)
            {
                // Autenticación exitosa
                Session["UserId"] = userId;
                return RedirectToAction("Index", "Viaticos");
            }
            else if (userId == 0)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            }
            else if (userId == -1)
            {
                ModelState.AddModelError("", "Usuario no encontrado en la base de datos.");
            }
            else
            {
                ModelState.AddModelError("", "Error en la autenticación.");
            }

            return View(user);
        }
        public ActionResult Logout()
        {
            Session.Clear(); // Limpiar los datos de la sesión
            return RedirectToAction("Login", "Account"); // Redirigir al login
        }
    }
}
