using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Collections.Generic;
using ViaticosWeb.Models;
using System.Configuration;

namespace VehiculosCombustible.Controllers
{
    
    public class VehiculoCombustibleController : Controller
    {


        private readonly string connectionString = "data source=192.168.53.43;initial catalog=Viaticos;user id=practact;password=Sistemas2024;";

        public ActionResult Index()
        {
            var model = new VehiculoCombustible();
            CargarGrifos();
            CargarGerencias();
            return View(model);
        }

        private void CargarGrifos()
        {
            var grifos = new List<Grifo>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT item, descripcion FROM [Viaticos].[dbo].[VehGrifos] WHERE estado ='A' ORDER BY 1 DESC";

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            grifos.Add(new Grifo
                            {
                                Item = Convert.ToInt32(dr["item"]),
                                Descripcion = dr["descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            ViewBag.Grifos = new SelectList(grifos, "Item", "Descripcion");
        }

        private void CargarGerencias()
        {
            var areas = new List<Area>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT arecod, arenom FROM [Viaticos].[dbo].[VehAreas] WHERE aredep = 0 and estado ='A'";

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            areas.Add(new Area
                            {
                                AreCod = Convert.ToInt32(dr["arecod"]),
                                AreNom = dr["arenom"].ToString()
                            });
                        }
                    }
                }
            }
            ViewBag.Gerencias = new SelectList(areas, "AreCod", "AreNom");
        }

        public JsonResult ObtenerUnidades(int gerenciaId)
        {
            var unidades = new List<Unidad>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT [ARECOD],[AREDEP],[ARENOM] FROM [Viaticos].[dbo].[VehAreas] " +
                                    "WHERE ESTADO = 'A' AND AREDEP = @GerenciaId order by arenom asc";
                    cmd.Parameters.AddWithValue("@GerenciaId", gerenciaId);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            unidades.Add(new Unidad
                            {
                                AreCod = Convert.ToInt32(dr["ARECOD"]),
                                AreDep = Convert.ToInt32(dr["AREDEP"]),
                                AreNom = dr["ARENOM"].ToString()
                            });
                        }
                    }
                }
            }
            return Json(unidades, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerPlacas(int unidadId)
        {
            var placas = new List<Placa>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT ITEM, VEHPLA FROM [Viaticos].[dbo].[VehMae] " +
                                    "WHERE ESTADO = 'A' and vehUnid = @UnidadId";
                    cmd.Parameters.AddWithValue("@UnidadId", unidadId);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            placas.Add(new Placa
                            {
                                Item = Convert.ToInt32(dr["ITEM"]),
                                VehPla = dr["VEHPLA"].ToString()
                            });
                        }
                    }
                }
            }
            return Json(placas, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDetalleVehiculo(int placaId)
        {
            var vehiculo = new VehiculoCombustible();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT C.MARNOM, A.VEHANO, D.DESCRIPCION, B.ARENOM, A.ASIGNADO, A.VEHEST, 
                                    E.Descrpcion, A.CODACT, A.Vo2Cod
                                    FROM [Viaticos].[dbo].[VehMae] A 
                                    INNER JOIN [Viaticos].[dbo].[VehAreas] B ON A.ARECOD = B.ARECOD and b.estado = 'A'
                                    INNER JOIN [Viaticos].[dbo].[Vehmar] C ON A.marcod = c.Item and C.estado = 'A'
                                    INNER JOIN [Viaticos].[dbo].[VehTip] D ON A.tipcod = D.ITEM and D.estado = 'A'
                                    INNER JOIN [Viaticos].[dbo].[VehTipCom] E ON A.vo2cod = E.ITEM and E.estado = 'A'
                                    WHERE A.ESTADO ='A' and A.Item = @PlacaId";
                    cmd.Parameters.AddWithValue("@PlacaId", placaId);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            vehiculo = new VehiculoCombustible
                            {
                                Marca = dr["MARNOM"].ToString(),
                                AnioVehiculo = dr["VEHANO"].ToString(),
                                TipoVehiculo = dr["DESCRIPCION"].ToString(),
                                AreaAsignada = dr["ARENOM"].ToString(),
                                Asignado = dr["ASIGNADO"].ToString(),
                                Estado = dr["VEHEST"].ToString(),
                                Combustible = dr["Descrpcion"].ToString(),
                                CodigoActivo = dr["CODACT"].ToString(),
                                CodigoCombustible = dr["Vo2Cod"].ToString()
                            };
                        }
                    }
                }
            }
            return Json(vehiculo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult Grabar(VehiculoCombustible model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            try
            {
                // Obtener el 'userId' de la sesión
                int usuariof = (int)Session["UserId"];  // Esto debe estar guardado previamente en la sesión al momento de iniciar sesión

                // Verificar si el usuario está autenticado y el 'userId' es válido
                if (usuariof <= 0)
                {
                    return Json(new { success = false, message = "Usuario no autenticado o ID inválido." });
                }

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Declarar variables necesarias
                            string tipmed;
                            int tipoCombustible;

                            // Determinar valores según la lógica de codact
                            if (model.CodigoActivo == "991" || model.CodigoActivo == "992")
                            {
                                tipmed = "LITROS";
                                tipoCombustible = model.TipoCombustibleId + 4;
                            }
                            else
                            {
                                tipmed = "GALONES";
                                tipoCombustible = Convert.ToInt32(model.CodigoCombustible);
                            }

                            // Ejecutar el procedimiento almacenado [dbo].[Veh_SolCab]
                            var cmd = new SqlCommand("[dbo].[Veh_SolCab]", conn, transaction)
                            {
                                CommandType = CommandType.StoredProcedure
                            };
                            cmd.Parameters.Add("@tipo", SqlDbType.VarChar, 1).Value = "V";
                            cmd.Parameters.Add("@codveh", SqlDbType.VarChar, 50).Value = model.PlacaId;
                            cmd.Parameters.Add("@kmant", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@kmact", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@km", SqlDbType.Decimal).Value = model.Kilometraje;
                            cmd.Parameters.Add("@tipman", SqlDbType.Int).Value = 3; 
                            cmd.Parameters.Add("@observaciones", SqlDbType.VarChar, 200).Value = "VALE DE COMBUSTIBLE";
                            cmd.Parameters.Add("@codgrifo", SqlDbType.Int).Value = model.GrifoId;
                            cmd.Parameters.Add("@usureg", SqlDbType.Int).Value = usuariof; // Usamos el 'userId' aquí, obtenido de la sesión
                            cmd.Parameters.Add("@gercod", SqlDbType.Int).Value = model.GerenciaId;
                            cmd.Parameters.Add("@arecod", SqlDbType.Int).Value = model.UnidadId;
                            cmd.Parameters.Add("@anno", SqlDbType.Int).Value = DateTime.Now.Year;
                            cmd.Parameters.Add("@tipcomb", SqlDbType.Int).Value = tipoCombustible;

                            // Obtener el resultado del procedimiento
                            int idHeader = Convert.ToInt32(cmd.ExecuteScalar());

                            // Actualizar maestro de vehículos
                            var updateCmd = new SqlCommand(
                                "UPDATE [Viaticos].[dbo].[VehMae] SET [VehKm] = @Kilometraje WHERE ITEM = @PlacaId",
                                conn,
                                transaction
                            );
                            updateCmd.Parameters.AddWithValue("@Kilometraje", model.Kilometraje);
                            updateCmd.Parameters.AddWithValue("@PlacaId", model.PlacaId);
                            updateCmd.ExecuteNonQuery();

                            // Insertar detalle
                            var insertCmd = new SqlCommand(
                                @"INSERT INTO [Viaticos].[dbo].[VehDet] 
                                      (solcod, codmot, descripcion, cantidad, anno, TipMed)
                                      VALUES (@IdHeader, 4, 'COMBUSTIBLE', @Cantidad, @Anio, @TipMed)",
                                conn,
                                transaction
                            );
                            insertCmd.Parameters.AddWithValue("@IdHeader", idHeader);
                            insertCmd.Parameters.AddWithValue("@Cantidad", model.Cantidad);
                            insertCmd.Parameters.AddWithValue("@Anio", DateTime.Now.Year);
                            insertCmd.Parameters.AddWithValue("@TipMed", tipmed);
                            insertCmd.ExecuteNonQuery();

                            // Confirmar transacción
                            transaction.Commit();
                            return Json(new { success = true, message = $"Se generó el Vale de combustible: {idHeader}" });
                        }
                        catch (Exception ex)
                        {
                            // Revertir transacción en caso de error
                            transaction.Rollback();
                            return Json(new { success = false, message = $"Error en la transacción: {ex.Message}" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error general: {ex.Message}" });
            }
        }
    }
}