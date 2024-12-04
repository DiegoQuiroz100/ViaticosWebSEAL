using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using ViaticosWeb.Models;

namespace ViaticosWeb.Controllers
{
    
    public class RevisionValeController : Controller
    {
        private readonly string connectionString = "data source=192.168.53.43;initial catalog=Viaticos;user id=practact;password=Sistemas2024;";

        // Método para obtener el código de gerencia del usuario actual
        private string ObtenerCodGerencia()
        {
            int usuariof = (int)Session["UserId"]; // Usuario de la sesión
            string codger = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT areusr FROM [AdmUsu].[dbo].[tbl_Usuarios] WHERE estusr = 'A' AND ideusr = @usuariof";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@usuariof", usuariof);

                try
                {
                    connection.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        codger = result.ToString();  // Obtener el código de gerencia
                    }
                }
                catch (SqlException ex)
                {
                    TempData["ErrorMessage"] = $"Error al obtener código de gerencia: {ex.Message}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Ocurrió un error: {ex.Message}";
                }
            }

            return codger;
        }

        // GET: RevisionVale
        public ActionResult Index()
        {
            var vales = new List<RevisionVale>();
            string codger = ObtenerCodGerencia();  // Obtener el código de gerencia

            if (string.IsNullOrEmpty(codger))
            {
                TempData["ErrorMessage"] = "No se pudo obtener el código de gerencia.";
                return RedirectToAction("Error");
            }

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
                    WHERE A.TIPO = 'V' AND A.estado = 'A' AND A.gercod = @Gerencia
                    ORDER BY 1 ASC";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Gerencia", codger);  // Filtrar por código de gerencia

                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var vale = new RevisionVale
                        {
                            Solicitud = reader["solcod"].ToString(),
                            Marca = reader["MarNom"].ToString(),
                            Placa = reader["vehpla"].ToString(),
                            Asignado = reader["Asignado"].ToString(),
                            Descripcion = reader["descripcion"].ToString(),
                            KM = Convert.ToInt32(reader["km"]),
                            Cantidad = Convert.ToInt32(reader["cantidad"]),
                            Unidad = reader["tipmed"].ToString(),
                            Grifo = reader["grifo"].ToString()
                        };
                        vales.Add(vale);
                    }
                }
                catch (SqlException ex)
                {
                    TempData["ErrorMessage"] = $"Error al cargar los vales: {ex.Message}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Ocurrió un error: {ex.Message}";
                }
            }

            return View(vales);
        }

        // Método para revisar los vales seleccionados
        [HttpPost]
        public ActionResult Revisar(List<int> seleccionados)
        {
            if (seleccionados == null || seleccionados.Count == 0)
            {
                TempData["ErrorMessage"] = "No se seleccionaron vales para revisar.";
                return RedirectToAction("Index");
            }

            try
            {
                int usuariof = (int)Session["UserId"]; // Usuario de la sesión
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var solcod in seleccionados)
                    {
                        string query = "UPDATE [Viaticos].[dbo].[VehCab] SET Fecrev = GETDATE(), usurev = @usuariof, estado = 'R' WHERE solcod = @solcod";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@usuariof", usuariof); // Código del usuario
                            cmd.Parameters.AddWithValue("@solcod", solcod);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                TempData["SuccessMessage"] = "Vales revisados con éxito.";
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = $"Error en la base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // Método para anular los vales seleccionados
        [HttpPost]
        public ActionResult Anular(List<int> seleccionados)
        {
            if (seleccionados == null || seleccionados.Count == 0)
            {
                TempData["ErrorMessage"] = "No se seleccionaron vales para anular.";
                return RedirectToAction("Index");
            }

            try
            {
                int usuariof = (int)Session["UserId"]; // Usuario de la sesión

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var solcod in seleccionados)
                    {
                        string query = "UPDATE [Viaticos].[dbo].[VehCab] SET fecanu = GETDATE(), usuanu = @usuariof, estado = 'X' WHERE solcod = @solcod";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@usuariof", usuariof); // Código del usuario
                            cmd.Parameters.AddWithValue("@solcod", solcod);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                TempData["SuccessMessage"] = "Vales anulados con éxito.";
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = $"Error en la base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // Método de error general
        public ActionResult Error()
        {
            return View();
        }
    }
}
