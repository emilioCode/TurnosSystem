using System;
using System.Collections.Generic;

namespace TurnosSystem.Models
{
    public partial class Servicios
    {
        public int Id { get; set; }
        public string TipoServicio { get; set; }
        public int? TiempoEstimado { get; set; }
        public string CodigoServicio { get; set; }
    }
}
