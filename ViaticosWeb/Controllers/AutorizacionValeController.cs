using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using ViaticosWeb.Models; //

namespace ViaticosWeb.Controllers
{
    
    public class AutorizacionValeController : Controller
    {
        // Cadena de conexión especificada
        private readonly string connectionString = "data source=192.168.53.43;initial catalog=Viaticos;user id=practact;password=Sistemas2024;";

        // Acción para mostrar la lista de vales de combustible
        public ActionResult Index()
        {
            var vales = ObtenerValesGenerados();
            return View(vales);
        }

        // Método para obtener el código de gerencia del usuario actual
        private int ObtenerCodigoGerencia()
        {
            int codger = 0;  // Valor por defecto

            int usuariof = (int)Session["UserId"]; // Obtener el ID de usuario desde la sesión

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT areusr FROM [AdmUsu].[dbo].[tbl_Usuarios] WHERE estusr = 'A' AND ideusr = @usuariof";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@usuariof", usuariof);

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        codger = Convert.ToInt32(reader["areusr"]);  // Obtener el código de gerencia
                    }
                }
            }

            return codger;
        }

        // Método para obtener los vales generados para la gerencia del usuario
        private List<AutorizacionVale> ObtenerValesGenerados()
        {
            var vales = new List<AutorizacionVale>();

            int codger = ObtenerCodigoGerencia();  // Obtener el código de gerencia del usuario

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT a.solcod, F.MarNom, c.vehpla, c.Asignado, d.descripcion, a.km, b.cantidad, b.tipmed, e.descripcion AS grifo
                    FROM [Viaticos].[dbo].[VehCab] A 
                    INNER JOIN [Viaticos].[dbo].[VehDet] B ON A.SOLCOD = B.SOLCOD
                    INNER JOIN [Viaticos].[dbo].[VehMae] C ON A.codveh = C.Item AND C.estado = 'A'
                    INNER JOIN [Viaticos].[dbo].[VehMot] D ON A.tipman = D.item
                    INNER JOIN [Viaticos].[dbo].[VehGrifos] E ON A.codgrifo = e.item
                    INNER JOIN [Viaticos].[dbo].[VehMar] F ON C.MarCod = F.Item
                    WHERE A.TIPO = 'V' AND A.estado = 'G' AND A.gercod = @Gerencia
                    ORDER BY a.solcod ASC";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Gerencia", codger);

                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vales.Add(new AutorizacionVale
                        {
                            Solcod = Convert.ToInt32(reader["solcod"]),
                            Marca = reader["MarNom"].ToString(),
                            Placa = reader["vehpla"].ToString(),
                            Asignado = reader["Asignado"].ToString(),
                            Descripcion = reader["descripcion"].ToString(),
                            KM = Convert.ToInt32(reader["km"]),
                            Cantidad = Convert.ToInt32(reader["cantidad"]),
                            Unidad = reader["tipmed"].ToString(),
                            Grifo = reader["grifo"].ToString()
                        });
                    }
                }
            }

            return vales;
        }

        // Acción para autorizar los vales seleccionados
        [HttpPost]
        public ActionResult Autorizar(List<int> seleccionados)
        {
            if (seleccionados == null || seleccionados.Count == 0)
            {
                TempData["ErrorMessage"] = "Debe seleccionar al menos un vale para autorizar.";
                return RedirectToAction("Index");
            }

            int usuariof = (int)Session["UserId"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var solcod in seleccionados)
                {
                    string query = "UPDATE [Viaticos].[dbo].[VehCab] " +
                                   "SET Fecaut = GETDATE(), usuaut = @usuariof, estado = 'A' " +
                                   "WHERE solcod = @solcod";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@usuariof", usuariof); // Código del usuario.
                    cmd.Parameters.AddWithValue("@solcod", solcod);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["SuccessMessage"] = "Vales autorizados con éxito.";
            return RedirectToAction("Index");
        }

        // Acción para anular los vales seleccionados
        [HttpPost]
        public ActionResult Anular(List<int> seleccionados)
        {
            if (seleccionados == null || seleccionados.Count == 0)
            {
                TempData["ErrorMessage"] = "Debe seleccionar al menos un vale para anular.";
                return RedirectToAction("Index");
            }

            int usuariof = (int)Session["UserId"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var solcod in seleccionados)
                {
                    string query = @"
                        UPDATE [Viaticos].[dbo].[VehCab] 
                        SET fecanu = GETDATE(), usuanu = @usuariof, estado = 'X' 
                        WHERE solcod = @solcod";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@solcod", solcod);
                    cmd.Parameters.AddWithValue("@usuariof", usuariof); // Código del usuario.
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["SuccessMessage"] = "Vales anulados con éxito.";
            return RedirectToAction("Index");
        }
    }
}
