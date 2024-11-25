using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Security.Cryptography;
using System.Text;

namespace ViaticosWeb.Models
{
    public class UserAuthenticator
    {
        private readonly string _connectionString;

        public UserAuthenticator(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int ValidateUser(string username, string password)
        {
            int userId = -1;

            // Primero, validar las credenciales en Active Directory
            if (!AuthenticateLDAP(username, password))
            {
                return 0; // Credenciales inválidas en LDAP
            }

            // Si LDAP es exitoso, validar en la base de datos
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("[dbo].[pro_Valsur]", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetros
                command.Parameters.AddWithValue("@IDUSUARIO", username);
                command.Parameters.AddWithValue("@IPASSWORD", EncryptSHA1(password));

                SqlParameter outputParam = new SqlParameter("@OIDUSUARIO", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    userId = Convert.ToInt32(outputParam.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return userId;
        }

        private bool AuthenticateLDAP(string username, string password)
        {
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry("LDAP://SEALSCDC01", username, password))
                {
                    var nativeObject = entry.NativeObject; // Verifica si las credenciales son válidas
                    return true;
                }
            }
            catch
            {
                return false; // Credenciales inválidas
            }
        }

        private string EncryptSHA1(string input)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
