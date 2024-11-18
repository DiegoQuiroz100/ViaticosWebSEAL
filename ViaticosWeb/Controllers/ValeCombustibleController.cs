using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using ViaticosWeb.Models; // Ajusta el espacio de nombres según tu proyecto

namespace YourNamespace.Controllers
{
    public class ValeCombustibleController : Controller
    {
        private readonly string connectionString = "data source=192.168.53.43;initial catalog=Viaticos;user id=practact;password=Sistemas2024;";

        // Vista principal
        public ActionResult Index()
        {
            // Carga inicial de combos
            ViewBag.Grifos = GetGrifos();
            ViewBag.Areas = GetAreas();
            return View();
        }

        // Método para cargar las placas al seleccionar un área
        [HttpGet]
        public JsonResult GetPlacas(int unidadId)
        {
            var placas = new List<PlacaViewModel>();

            // Usamos using para asegurar el cierre de la conexión
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consulta SQL para obtener las placas
                var query = "SELECT ITEM, VEHPLA FROM [Viaticos].[dbo].[VehMae] WHERE ESTADO = 'A' AND vehUnid = 47";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UnidadId", unidadId);

                    // Ejecutamos la consulta
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            placas.Add(new PlacaViewModel
                            {
                                Id = Convert.ToInt32(reader["ITEM"]),
                                Placa = reader["VEHPLA"].ToString()
                            });
                        }
                    }
                }
            }

            return Json(placas, JsonRequestBehavior.AllowGet);
        }

        // Método para obtener los datos de un vehículo al seleccionar la placa
       [HttpGet]
            public JsonResult GetVehiculoDetails(int placaId)
        {
            VehiculoViewModel vehiculo = null;

            // Usamos using para asegurar el cierre de la conexión
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consulta SQL para obtener los detalles del vehículo
                var query = @"SELECT C.MARNOM, A.VEHANO, D.DESCRIPCION, B.ARENOM, A.ASIGNADO, A.VEHEST, E.Descrpcion
                              FROM [Viaticos].[dbo].[VehMae] A
                              INNER JOIN [Viaticos].[dbo].[VehAreas] B ON A.ARECOD = B.ARECOD AND B.ESTADO = 'A'
                              INNER JOIN [Viaticos].[dbo].[Vehmar] C ON A.MARCOD = C.ITEM AND C.ESTADO = 'A'
                              INNER JOIN [Viaticos].[dbo].[VehTip] D ON A.TIPCOD = D.ITEM AND D.ESTADO = 'A'
                              INNER JOIN [Viaticos].[dbo].[VehTipCom] E ON A.VO2COD = E.ITEM AND E.ESTADO = 'A'
                              WHERE A.ESTADO = 'A' AND A.ITEM = @PlacaId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlacaId", placaId);

                    // Ejecutamos la consulta
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            vehiculo = new VehiculoViewModel
                            {
                                Marca = reader["MARNOM"].ToString(),
                                Anio = reader["VEHANO"].ToString(),
                                Tipo = reader["DESCRIPCION"].ToString(),
                                AreaAsignada = reader["ARENOM"].ToString(),
                                Asignado = reader["ASIGNADO"].ToString(),
                                Estado = reader["VEHEST"].ToString(),
                                Combustible = reader["Descrpcion"].ToString()
                            };
                        }
                    }
                }
            }

            return Json(vehiculo, JsonRequestBehavior.AllowGet);
        }

        // Guardar el vale de combustible
        [HttpPost]
        public JsonResult GuardarVale(ValeCombustibleViewModel model)
        {
            try
            {
                if (model.Kilometraje <= 0 || model.Cantidad <= 0)
                {
                    return Json(new { success = false, message = "Los campos Kilometraje y Cantidad deben ser mayores a cero." });
                }

                int idVale;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Inserta el registro en la tabla de cabecera
                    var queryHeader = @"INSERT INTO [Viaticos].[dbo].[Veh_SolCab] (/* Campos relevantes */)
                                        OUTPUT INSERTED.ID
                                        VALUES (/* Valores */)";
                    using (var command = new SqlCommand(queryHeader, connection))
                    {
                        // Agrega parámetros
                        command.Parameters.AddWithValue("@Kilometraje", model.Kilometraje);
                        command.Parameters.AddWithValue("@Cantidad", model.Cantidad);
                        idVale = (int)command.ExecuteScalar();
                    }

                    // Actualizar kilometraje
                    var updateQuery = "UPDATE [Viaticos].[dbo].[VehMae] SET VehKm = @Kilometraje WHERE ITEM = @PlacaId";
                    using (var updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Kilometraje", model.Kilometraje);
                        updateCommand.Parameters.AddWithValue("@PlacaId", model.PlacaId);
                        updateCommand.ExecuteNonQuery();
                    }

                    // Inserta el detalle
                    var insertDetailQuery = @"INSERT INTO [Viaticos].[dbo].[VehDet] (/* Campos relevantes */)
                                              VALUES (/* Valores */)";
                    using (var detailCommand = new SqlCommand(insertDetailQuery, connection))
                    {
                        // Agrega parámetros
                        detailCommand.Parameters.AddWithValue("@IdVale", idVale);
                        detailCommand.Parameters.AddWithValue("@Cantidad", model.Cantidad);
                        detailCommand.ExecuteNonQuery();
                    }
                }

                return Json(new { success = true, message = $"Vale guardado correctamente con ID: {idVale}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // Métodos auxiliares para cargar combos
        private List<GrifoViewModel> GetGrifos()
        {
            var grifos = new List<GrifoViewModel>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT item, descripcion FROM [Viaticos].[dbo].[VehGrifos] WHERE estado = 'A' ORDER BY 1 DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grifos.Add(new GrifoViewModel
                            {
                                Id = Convert.ToInt32(reader["item"]),
                                Descripcion = reader["descripcion"].ToString()
                            });
                        }
                    }
                }
            }

            return grifos;
        }

        private List<AreaViewModel> GetAreas()
        {
            var areas = new List<AreaViewModel>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT arecod, arenom FROM [Viaticos].[dbo].[VehAreas] WHERE aredep = 0 AND estado = 'A'";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            areas.Add(new AreaViewModel
                            {
                                Id = Convert.ToInt32(reader["arecod"]),
                                Nombre = reader["arenom"].ToString()
                            });
                        }
                    }
                }
            }

            return areas;
        }
    }
}
