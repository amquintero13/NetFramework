using System;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace ConsoleApp1NetFramework
{
    class Program
    {
        private static string connectionString;
        private static string logFile;
        private static string errorLogFile;

        static void Main(string[] args)
        {
            // Leer configuración del archivo XML y setear variables globales de clase
            XmlDocument doc = new XmlDocument();
            doc.Load("Config.xml");
            connectionString = doc.SelectSingleNode("/config/connectionString").InnerText;
            logFile = doc.SelectSingleNode("/config/logFile").InnerText;
            errorLogFile = doc.SelectSingleNode("/config/errorLogFile").InnerText;

            //Guardar Usuarios
            Usuario usuario1 = new Usuario("Pedro", "Perez", 26, "pruebas@hotmail.com", "Nadar-Correr-Jugar fútbol-Cocinar");
            GuardarUsuario(usuario1);
            Usuario usuario2 = new Usuario("luisa", "rojas", 15, "pruebas2@hotmail.com", "Caminar-Baloncesto-Television-Musica");
            GuardarUsuario(usuario2);
            Usuario usuario3 = new Usuario("Rosa", "Aguirre", 38, "pruebas4@hotmail.com", "Dormir-Cocinar-Comer");
            GuardarUsuario(usuario3);


            // Lanzar procedmiento por edades
            ObtenerUsuariosPorEdad(12);
            ObtenerUsuariosPorEdad(30);

            // Lanzar procedimiento Por Horas
            ObtenerUsuariosUltimas2Horas();
        }

        private static void GuardarUsuario(Usuario pUsuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "sp_InsertarUsuario" + " '" + pUsuario.Nombre + "','" + pUsuario.Apellido + "'," + pUsuario.Edad +
                                      ",'" + pUsuario.Correo + "','" + pUsuario.Hobbies + "','Admin'";
                    SqlCommand cmd1 = new SqlCommand(sqlQuery, connection);
                    SqlDataReader reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        if (string.IsNullOrEmpty(reader1[0].ToString()))
                        {
                            EscribirLog("Usuario: " + pUsuario.ObtenerNombreCompleto());
                        }
                        else
                        {
                            EscribirErrorLog(reader1[0].ToString());
                        }
                    }

                    reader1.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                EscribirErrorLog(ex.Message);
            }
        }

        private static void ObtenerUsuariosPorEdad(int pEdadMinima)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "sp_ObtenerUsuariosPorEdad " + pEdadMinima;
                    SqlCommand cmd1 = new SqlCommand(sqlQuery, connection);
                    SqlDataReader reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        EscribirLog("Usuario: " + reader1["IdUsuario"] + " - " + reader1["Nombre"] + " - " + reader1["Apellido"] + " - " + reader1["Edad"] +
                                    " - " + reader1["Correo"] + " - " + reader1["HobbiesOrdenados"] + " - " + (reader1["Activo"].Equals(1) ? "SI" : "NO") + " - " + reader1["FechaCreacion"]);
                    }

                    reader1.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                EscribirErrorLog(ex.Message);
            }
        }

        private static void ObtenerUsuariosUltimas2Horas()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd1 = new SqlCommand("sp_ObtenerUsuariosUltimas2Horas", connection);
                    SqlDataReader reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        EscribirLog("Usuario: " + reader1["IdUsuario"] + " - " + reader1["Nombre"] + " - " + reader1["Apellido"] + " - " + reader1["Edad"] +
                                    " - " + reader1["Correo"] + " - " + reader1["HobbiesOrdenados"] + " - " + (reader1["Activo"].Equals(1) ? "SI" : "NO") + " - " + reader1["FechaCreacion"]);
                    }

                    reader1.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                EscribirErrorLog(ex.Message);
            }
        }


        private static void EscribirLog(string pLinea)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine("Fecha:" + DateTime.Now + " - " + pLinea);
                }
            }
            catch (Exception ex)
            {
                EscribirErrorLog(ex.Message);
            }
        }

        private static void EscribirErrorLog(string pLineaError)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(errorLogFile, true))
                {
                    writer.WriteLine("Fecha:" + DateTime.Now + " - " + pLineaError);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
