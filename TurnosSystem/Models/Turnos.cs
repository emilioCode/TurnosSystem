using System;
using System.Collections.Generic;

namespace TurnosSystem.Models
{
    public partial class Turnos
    {
        public int Id { get; set; }
        public int TipoServicio { get; set; }
        public int Turno { get; set; }
        public string Fecha { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public bool? Despachado { get; set; }
        public bool? Habilitado { get; set; }
        public bool? Atendido { get; set; }
        public int? Usuario { get; set; }
    }
}
