using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemasTurnos.Models;
using TurnosSystem.Models;


namespace TurnosSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {

        private readonly dbServicioTurnosContext context;

        public ServiciosController(dbServicioTurnosContext _context)
        {
            this.context = _context;
        }

        // GET: api/Servicios
        [HttpGet]
        public IEnumerable<Servicios> Get()
        {
            //return new string[] { "value1", "value2" };
            return context.Servicios.ToList();
        }

        //// GET: api/Servicios/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        public class LoginUser
        {
            public string user { get; set; }
            public string pwd { get; set; }
        }

        [HttpPost("[action]")]
        public JsonResult Logearse( LoginUser loginUser)
        {
            //var usuario = context.Usuarios.Where(u => u.UserName == user && u.Password == null).SingleOrDefault();
            var usuario = context.Usuarios.Where(u => u.UserName == loginUser.user && u.Password == loginUser.pwd).SingleOrDefault();
            return new JsonResult(usuario);  
        }
        [HttpPost("[action]")]
        public JsonResult getTickets()
        {
            List<fmtTurnos> lista = new List<fmtTurnos>();
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var tickets = context.Turnos.Where(t => t.Fecha == fecha && (t.Despachado == null || t.Despachado == false) && t.Habilitado == true && (t.Atendido == null || t.Atendido == false)).ToList();
            int turno = 1;
            
            foreach (var item in tickets)
            {
                lista.Add(new fmtTurnos
                {
                    id = item.Id,
                    TipoServicio = item.TipoServicio,
                    Turno = item.Turno,
                    Fecha = item.Fecha,
                    Hora_inicio = item.HoraInicio,
                    Hora_fin = item.HoraFin,
                    Despachado = item.Despachado,
                    Habilitado = item.Habilitado,
                    atendido = item.Atendido,
                    usuario = item.Usuario,
                    no = turno
                });
                turno++;
            }

            return new JsonResult(lista);  
        }

        //public class turnoDespachar
        //{
        //    public int ticket { get; set; }
        //    public int user { get; set; }
        //}
        [HttpPost("[action]")]
        public JsonResult atender([FromBody]turnoDespachar data)
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var hora = DateTime.Now.ToString().Split(' ')[1];

            var turno = context.Turnos.Find(data.ticket);
            turno.Usuario = data.user;
            turno.Atendido = true;
            turno.HoraInicio = hora;
            context.Entry(turno).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return new JsonResult(turno);
        }

        [HttpPost("[action]")]
        public void deshabilitar([FromBody]int id)
        {
            var turno = context.Turnos.Find(id);
            turno.Habilitado = false;
            context.Entry(turno).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        // POST: api/Servicios
        [HttpPost]
        public string Post([FromBody] int id)   //setTurno
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var hoy = DateTime.Now.ToShortTimeString();
            var hora = Convert.ToInt32(hoy.ToString().Split(':')[0]);
            var tanda = hoy.ToString().Split(' ')[1];
            //var tanda2 = today.
            if ((hora >= 12 && (tanda == "a.m." || tanda == "A.M.")) || (hora >= 6 && tanda == "p.m." || tanda == "P.M."))
            {

                return "Sistema de turnos cerrado, vuelva luego";
            }

            var currentService = context.Turnos.Where(t => t.TipoServicio == id && t.Fecha == fecha)
                .OrderByDescending(t => t.Id)
                .Select(t => t.Turno).FirstOrDefault();
            int order = 1;
            if (currentService != null) order = currentService + order;

            var turno = new Turnos();
            turno.TipoServicio = id;
            turno.Fecha = fecha;
            turno.Habilitado = true;
            turno.Turno = order;

            context.Turnos.Add(turno);
            context.SaveChanges();

            var count = context.Turnos.Where(t => t.Fecha == fecha
            && t.TipoServicio == id
            && t.HoraInicio == null).Count();

            var codService = context.Servicios.Where(s => s.Id == id).FirstOrDefault();

            //var totalturnos = db.Turnos.Where(t => t.id == id && t.Habilitado == true
            // && t.Fecha == fecha /*&& t.Despachado != true*/).ToList();

            int mins = Convert.ToInt32(codService.TiempoEstimado);
            //var tiempoEstimado = mins * (order);
            var tiempoEstimado = mins * (count - 1);

            return $"{ codService.CodigoServicio }-{ order } { tiempoEstimado }";

        }

        //// PUT: api/Servicios/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        [HttpPost("[action]")]
        public JsonResult getTicketsFinishes()
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var tickets = context.Turnos.Where(t => t.Fecha == fecha && (t.Despachado == null || t.Despachado == false) && t.Habilitado == true && t.Atendido == true).ToList();
            return new JsonResult(tickets);
        }

        [HttpPost("[action]")]
        public void despachar([FromBody]int id)
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var hora = DateTime.Now.ToString().Split(' ')[1];

            var turno = context.Turnos.Find(id);
            turno.Despachado = true;
            turno.HoraFin = hora;
            context.Entry(turno).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        [HttpPost("[action]")]
        public JsonResult getTicketsforUser()
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var turnos = from a in context.Turnos.ToList()
                         join b in context.Usuarios.ToList() on a.Usuario equals b.Id
                         join c in context.Servicios.ToList() on a.TipoServicio equals c.Id
                         where a.Despachado == true && a.Habilitado == true && a.Atendido == true
                         && a.Fecha == fecha
                         select new
                         {
                             tipoServicio = c.CodigoServicio,
                             turno = a.Turno,
                             fecha = a.Fecha,
                             hora_inicio = a.HoraInicio,
                             hora_fin = a.HoraFin,
                             despachado = a.Despachado,
                             habilitado = a.Habilitado,
                             usuario = b.NombreCompleto,
                             atendido = a.Atendido
                         };

            return new JsonResult(turnos.ToList());
        }





    }
}
