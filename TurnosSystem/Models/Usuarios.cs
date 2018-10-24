using System;
using System.Collections.Generic;

namespace TurnosSystem.Models
{
    public partial class Usuarios
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Cedula { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Rol { get; set; }
    }
}
