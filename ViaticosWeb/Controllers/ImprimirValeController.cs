using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ClosedXML.Excel; // Asegúrate de tener ClosedXML para crear el archivo Excel
using ViaticosWeb.Models;


namespace ViaticosWeb.Controllers
{
    [CustomAuthorize]
    public class ImprimirValeController : Controller
    {
        private readonly string connectionString = "data source=192.168.53.43;initial catalog=Viaticos;user id=practact;password=Sistemas2024;";


        public ActionResult Index()
        {
            List<ImprimirValeModel> vehiculos = ImprimirVale();
            return View(vehiculos);
        }

        private List<ImprimirValeModel> ImprimirVale()
        {
            List<ImprimirValeModel> vehiculos = new List<ImprimirValeModel>();

            string query = @"SELECT a.solcod, F.MarNom, c.vehpla, c.Asignado, d.descripcion, a.km, b.cantidad, 
                                    e.descripcion AS grifo, c.vo2cod AS CodAct, b.tipmed AS TipoMedida 
                             FROM [Viaticos].[dbo].[VehCab] A 
                             INNER JOIN [Viaticos].[dbo].[VehDet] B ON A.SOLCOD = B.SOLCOD 
                             INNER JOIN [Viaticos].[dbo].[VehMae] C ON A.codveh = C.Item AND C.estado = 'A' 
                             INNER JOIN [Viaticos].[dbo].[VehMot] D ON A.tipman = D.item 
                             INNER JOIN [Viaticos].[dbo].[VehGrifos] E ON A.codgrifo = e.item 
                             INNER JOIN [Viaticos].[dbo].[VehMar] F ON C.MarCod = F.Item 
                             WHERE A.TIPO = 'V' AND a.estado = 'R'
                             ORDER BY 1 ASC";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {


                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ImprimirValeModel vehiculo = new ImprimirValeModel
                        {
                            SolCod = reader["solcod"].ToString(),
                            Marca = reader["MarNom"].ToString(),
                            Placa = reader["vehpla"].ToString(),
                            Asignado = reader["Asignado"].ToString(),
                            Descripcion = reader["descripcion"].ToString(),
                            KM = Convert.ToInt32(reader["km"]),
                            Cantidad = Convert.ToDecimal(reader["cantidad"]),
                            Grifo = reader["grifo"].ToString(),
                            CodAct = reader["CodAct"].ToString(),
                            TipoMedida = reader["TipoMedida"].ToString()
                        };
                        vehiculos.Add(vehiculo);
                    }
                }
            }

            return vehiculos;
        }

        [HttpPost]
        public ActionResult DescargarExcel(List<int> seleccionados)
        {
            if (seleccionados == null || seleccionados.Count == 0)
            {
                TempData["ErrorMessage"] = "Debe seleccionar al menos un vehículo para descargar.";
                return RedirectToAction("Index");
            }

            List<ImprimirValeModel> vehiculos = ImprimirVale().Where(v => seleccionados.Contains(int.Parse(v.SolCod))).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Vehiculos");

                // Configurar encabezados
                worksheet.Cell(1, 1).Value = "SolCod";
                worksheet.Cell(1, 2).Value = "Marca";
                worksheet.Cell(1, 3).Value = "Placa";
                worksheet.Cell(1, 4).Value = "Asignado";
                worksheet.Cell(1, 5).Value = "Descripcion";
                worksheet.Cell(1, 6).Value = "KM";
                worksheet.Cell(1, 7).Value = "Cantidad";
                worksheet.Cell(1, 8).Value = "Grifo";
                worksheet.Cell(1, 9).Value = "CodAct";
                worksheet.Cell(1, 10).Value = "TipoMedida";

                // Llenar datos
                for (int i = 0; i < vehiculos.Count; i++)
                {
                    var v = vehiculos[i];
                    worksheet.Cell(i + 2, 1).Value = v.SolCod;
                    worksheet.Cell(i + 2, 2).Value = v.Marca;
                    worksheet.Cell(i + 2, 3).Value = v.Placa;
                    worksheet.Cell(i + 2, 4).Value = v.Asignado;
                    worksheet.Cell(i + 2, 5).Value = v.Descripcion;
                    worksheet.Cell(i + 2, 6).Value = v.KM;
                    worksheet.Cell(i + 2, 7).Value = v.Cantidad;
                    worksheet.Cell(i + 2, 8).Value = v.Grifo;
                    worksheet.Cell(i + 2, 9).Value = v.CodAct;
                    worksheet.Cell(i + 2, 10).Value = v.TipoMedida;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    // Actualizar estado de los registros seleccionados
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        foreach (var solcod in seleccionados)
                        {
                            string query = "UPDATE [Viaticos].[dbo].[VehCab] SET Fecimp = GETDATE(), estado = 'I' WHERE solcod = @solcod";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            cmd.Parameters.AddWithValue("@solcod", solcod);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["SuccessMessage"] = "Descarga y actualización exitosa.";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListaValesImpresos.xlsx");
                }
            }

        }
    }
}