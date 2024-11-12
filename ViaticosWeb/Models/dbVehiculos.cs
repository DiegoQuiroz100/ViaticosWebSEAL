using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;

namespace ViaticosWeb.Models
{
    public class dbVehiculos
    {
        private readonly string connectionString = "data source=192.168.53.43;initial catalog=Viaticos;user id=practact;password=Sistemas2024;";

        public List<ListaVehiculos> ObtenerVehiculos()
        {
            List<ListaVehiculos> listaVehiculos = new List<ListaVehiculos>();

            using (SqlConnection con1 = new SqlConnection(connectionString))
            {
                string listquery = @"
                    SELECT a.Item, a.codact, a.VehPla, c.MarNom, d.Descripcion AS modelo, e.Descrpcion AS combustible, 
                           a.vehkm, a.VehEst, a.vehano, b.ARENOM, a.Asignado
                    FROM [Viaticos].[dbo].[VehMae] A 
                    INNER JOIN [Viaticos].[dbo].[VehAreas] B ON A.arecod = B.Arecod
                    INNER JOIN [Viaticos].[dbo].[VehMar] C ON A.Marcod = C.item
                    INNER JOIN [Viaticos].[dbo].[VehTip] D ON A.TipCod = D.item
                    INNER JOIN [Viaticos].[dbo].[VehTipCom] E ON A.Vo2Cod = E.item
                    WHERE a.estado = 'A' AND codemp = 1
                    ORDER BY a.Item ASC";

                using (SqlCommand cmd = new SqlCommand(listquery, con1))
                {
                    con1.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var vehiculo = new ListaVehiculos
                            {
                                Item = reader["Item"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Item"]),
                                CodAct = reader["codact"] == DBNull.Value ? "" : reader["codact"].ToString(),
                                VehPla = reader["VehPla"] == DBNull.Value ? "" : reader["VehPla"].ToString(),
                                MarNom = reader["MarNom"] == DBNull.Value ? "" : reader["MarNom"].ToString(),
                                Modelo = reader["modelo"] == DBNull.Value ? "" : reader["modelo"].ToString(),
                                Combustible = reader["combustible"] == DBNull.Value ? "" : reader["combustible"].ToString(),
                                VehKm = reader["vehkm"] == DBNull.Value ? 0 : Convert.ToInt32(reader["vehkm"]),
                                VehEst = reader["VehEst"] == DBNull.Value ? "" : reader["VehEst"].ToString(),
                                VehAno = reader["vehano"] == DBNull.Value ? 0 : Convert.ToInt32(reader["vehano"]),
                                AreaNom = reader["ARENOM"] == DBNull.Value ? "" : reader["ARENOM"].ToString(),
                                Asignado = reader["Asignado"] == DBNull.Value ? "" : reader["Asignado"].ToString()
                            };

                            listaVehiculos.Add(vehiculo);
                        }
                    }
                }
            }
            return listaVehiculos;
        }


        public List<ListaVehiculos> ObtenerHistorialVehiculo()
        {
            List<ListaVehiculos> historialVehiculo = new List<ListaVehiculos>();

            using (SqlConnection con1 = new SqlConnection(connectionString))
            {
                string listquery = @"
                        SELECT solcod, 
                               CASE WHEN tipo = 'M' THEN 'MANTENIMIENTO' ELSE 'COMBUSTIBLE' END AS tipo, 
                               a.fegreg, 
                               kmant, 
                               kmact, 
                               km, 
                               B.descripcion AS motivo, 
                               observaciones, 
                               C.descripcion AS grifo
                                FROM [Viaticos].[dbo].[VehCab] A 
                                INNER JOIN [Viaticos].[dbo].[VehMot] B ON A.tipman = B.Item 
                                LEFT JOIN [Viaticos].[dbo].[VehGrifos] C ON A.codgrifo = C.Item 
                                WHERE A.codveh = @codveh
                                ORDER BY solcod ASC";

                using (SqlCommand cmd = new SqlCommand(listquery, con1))
                {
                    con1.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var historial = new ListaVehiculos
                            {
                                SolCod = reader["solcod"] == DBNull.Value ? 0 : Convert.ToInt32(reader["solcod"]),
                                Tipo = reader["tipo"] == DBNull.Value ? "" : reader["tipo"].ToString(),
                                FechaRegistro = reader["fegreg"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["fegreg"]),
                                KmAnt = reader["kmant"] == DBNull.Value ? 0 : Convert.ToInt32(reader["kmant"]),
                                KmActual = reader["kmact"] == DBNull.Value ? 0 : Convert.ToInt32(reader["kmact"]),
                                Km = reader["km"] == DBNull.Value ? 0 : Convert.ToInt32(reader["km"]),
                                Motivo = reader["motivo"] == DBNull.Value ? "" : reader["motivo"].ToString(),
                                Observaciones = reader["observaciones"] == DBNull.Value ? "" : reader["observaciones"].ToString(),
                                Grifo = reader["grifo"] == DBNull.Value ? "" : reader["grifo"].ToString()
                            };

                            historialVehiculo.Add(historial);
                        }
                    }
                }
            }
            return historialVehiculo;
        }
        public List<ListaVales> ObtenerListaVales()
        {
            List<ListaVales> ListaVales = new List<ListaVales>();

            using (SqlConnection con1 = new SqlConnection(connectionString))
            {
                string listquery = @"
                            SELECT
                                A.solcod,
                                F.MarNom,
                                C.vehpla,
                                C.Asignado,
                                D.descripcion,
                                A.km,
                                B.cantidadmod AS cantidad,
                                E.descripcion AS grifo,
                                H.DESCRPCION AS vo2cod,
                                B.tipmed,
                                CASE
                                    WHEN A.ESTADO = 'G' THEN 'Generada'
                                    WHEN A.ESTADO = 'A' THEN 'Autorizado'
                                    WHEN A.ESTADO = 'R' THEN 'Revisado'
                                    WHEN A.ESTADO = 'X' THEN 'Anulado'
                                    WHEN A.ESTADO = 'F' THEN 'Facturado'
                                    WHEN A.ESTADO = 'I' THEN 'Impreso'
                                END AS Estado,
                                A.fecimp AS fegreg,
                                B.pu,
                                B.total,
                                G.NRODOC,
                                G.RUC,
                                I.ARENOM
                            FROM
                                [Viaticos].[dbo].[VehCab] A
                            INNER JOIN
                                [Viaticos].[dbo].[VehDet] B ON A.SOLCOD = B.SOLCOD
                            INNER JOIN
                                [Viaticos].[dbo].[VehMae] C ON A.codveh = C.Item
                            INNER JOIN
                                [Viaticos].[dbo].[VehMot] D ON A.tipman = D.item
                            INNER JOIN
                                [Viaticos].[dbo].[VehGrifos] E ON A.codgrifo = E.item
                            INNER JOIN
                                [Viaticos].[dbo].[VehMar] F ON C.MarCod = F.Item
                            INNER JOIN
                                [Viaticos].[dbo].[VehFacturacion] G ON G.IDGrupo = A.codgrupo
                            INNER JOIN
                                [Viaticos].[dbo].[VehTipCom] H ON C.vo2cod = H.ITEM
                            INNER JOIN
                                [Viaticos].[dbo].[VehAreas] I ON I.ARECOD = A.GERCOD
                            WHERE
                                A.ESTADO = 'F'
                                AND A.TIPO = 'V'
                            ORDER BY
                                    fegreg desc";

                using (SqlCommand cmd = new SqlCommand(listquery, con1))
                {
                    con1.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var vale = new ListaVales
                            {
                                SolCod = reader["solcod"] == DBNull.Value ? 0 : Convert.ToInt32(reader["solcod"]),
                                MarNom = reader["MarNom"] == DBNull.Value ? "" : reader["MarNom"].ToString(),
                                Vehpla = reader["vehpla"] == DBNull.Value ? "" : reader["vehpla"].ToString(),
                                Asignado = reader["Asignado"] == DBNull.Value ? "" : reader["Asignado"].ToString(),
                                Descripcion = reader["descripcion"] == DBNull.Value ? "" : reader["descripcion"].ToString(),
                                Km = reader["km"] == DBNull.Value ? 0 : Convert.ToInt32(reader["km"]),
                                Cantidad = reader["cantidad"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["cantidad"]),
                                Grifo = reader["grifo"] == DBNull.Value ? "" : reader["grifo"].ToString(),
                                Vo2cod = reader["vo2cod"] == DBNull.Value ? "" : reader["vo2cod"].ToString(),
                                TipMed = reader["tipmed"] == DBNull.Value ? "" : reader["tipmed"].ToString(),
                                Estado = reader["Estado"] == DBNull.Value ? "" : reader["Estado"].ToString(),
                                FecReg = reader["fegreg"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["fegreg"]),
                                Pu = reader["pu"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["pu"]),
                                Total = reader["total"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["total"]),
                                NroDoc = reader["NRODOC"] == DBNull.Value ? "" : reader["NRODOC"].ToString(),
                                Ruc = reader["RUC"] == DBNull.Value ? "" : reader["RUC"].ToString(),
                                AreNom = reader["ARENOM"] == DBNull.Value ? "" : reader["ARENOM"].ToString()
                            };

                            ListaVales.Add(vale);
                        }
                    }
                }
            }
            return ListaVales;
        }
    }
}








                    