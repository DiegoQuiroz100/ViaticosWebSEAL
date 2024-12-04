using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace ViaticosWeb.Models
{
    public class UserService
    {
        private readonly string connectionString;

        public UserService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int ValidateUser(string username, string password)
        {
            int userId = -1; // Valor predeterminado si no se valida el usuario

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EXECUTE [dbo].[pro_Valsur] @IDUSUARIO, @IPASSWORD, @OIDUSUARIO OUTPUT", conn))
                {
                    cmd.CommandType = CommandType.Text;

                    // Encriptar la contraseña usando SHA1 (mismo método que en VB)
                    string encryptedPassword = EncryptSHA1(password);

                    // Agregar los parámetros
                    cmd.Parameters.AddWithValue("@IDUSUARIO", username);
                    cmd.Parameters.AddWithValue("@IPASSWORD", encryptedPassword);

                    SqlParameter outputParam = new SqlParameter("@OIDUSUARIO", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // Abrir conexión y ejecutar
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // Obtener el valor de salida
                    userId = outputParam.Value != DBNull.Value ? Convert.ToInt32(outputParam.Value) : -1;
                }
            }

            return userId; // Devuelve -1 si el usuario no es válido
        }

        private string EncryptSHA1(string value)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(value);
                var hashBytes = sha1.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}