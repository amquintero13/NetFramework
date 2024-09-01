using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1NetFramework
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Correo { get; set; }
        public string Hobbies { get; set; }

        // Constructor
        public Usuario(string nombre, string apellido, int edad, string correo, string hobbies)
        {
            Nombre = nombre;
            Apellido = apellido;
            Edad = edad;
            Correo = correo;
            Hobbies = hobbies;
        }

        public string ObtenerNombreCompleto()
        {
            return Nombre + " " + Apellido;
        }
    }
}
