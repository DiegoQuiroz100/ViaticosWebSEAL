using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using ViaticosWeb.Models;

namespace ViaticosWeb.Controllers
{
    public class RevisionValeController : Controller
    {
        private readonly string connectionString = "data source=192.168.53.43;initial catalog=Viaticos;Integrated Security=True;";
        // GET: RevisionVale
        public ActionResult Index()
        {
            var vales = new List<RevisionVale>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT a.solcod, F.MarNom, c.vehpla, c.Asignado, d.descripcion, a.km, b.cantidad, b.tipmed, e.descripcion grifo 
                                 FROM [Viaticos].[dbo].[VehCab] A 
                                 INNER JOIN [Viaticos].[dbo].[VehDet] B ON A.SOLCOD = B.SOLCOD  
                                 INNER JOIN [Viaticos].[dbo].[VehMae] C ON A.codveh = C.Item AND C.estado = 'A'  
                                 INNER JOIN [Viaticos].[dbo].[VehMot] D ON A.tipman = D.item 
                                 INNER JOIN [Viaticos].[dbo].[VehGrifos] E ON A.codgrifo = e.item 
                                 INNER JOIN [Viaticos].[dbo].[VehMar] F ON C.MarCod = F.Item 
                                 WHERE	A.TIPO = 'V' and a.estado ='A'
                                 ORDER BY 1 ASC";

                SqlCommand cmd = new SqlCommand(query, connection);

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

            return View(vales);
        }
        [HttpPost]
        public ActionResult Revisar(List<int> seleccionados)
        {
            if (seleccionados == null || seleccionados.Count == 0)
            {
                TempData["ErrorMessage"] = "No se seleccionaron vales para revisar.";
                return RedirectToAction("Index");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                foreach (var id in seleccionados)
                {
                    string query = "UPDATE [Viaticos].[dbo].[VehCab] SET Fecrev = GETDATE(), estado = 'R' WHERE solcod = @solcod";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@solcod", id);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            TempData["SuccessMessage"] = "Vales revisados con éxito.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Anular(List<int> seleccionados)
        {
            if (seleccionados == null || seleccionados.Count == 0)
            {
                TempData["ErrorMessage"] = "No se seleccionaron vales para anular.";
                return RedirectToAction("Index");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                foreach (var id in seleccionados)
                {
                    string query = "UPDATE [Viaticos].[dbo].[VehCab] SET fecanu = GETDATE(), estado = 'X' WHERE solcod = @solcod";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@solcod", id);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            TempData["SuccessMessage"] = "Vales anulados con éxito.";
            return RedirectToAction("Index");
        }
    }
}