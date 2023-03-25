using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public reservasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<reservas> listadoReserva = (from e in _equiposContexto.reservas
                                             select e).ToList();

            if (listadoReserva.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoReserva);

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            var reserva = (from r in _equiposContexto.reservas
                           join e in _equiposContexto.equipos on r.equipo_id equals e.id_equipos
                           join u in _equiposContexto.usuarios on r.usuario_id equals u.usuario_id
                           join er in _equiposContexto.estados_reserva on r.estado_reserva_id equals er.estado_res_id
                           select new
                           {
                               r.reserva_id,
                               r.equipo_id,
                               e.nombre,
                               r.usuario_id,
                               nombre_usuario = u.nombre,
                               r.fecha_salida,
                               r.hora_salida,
                               r.tiempo_reserva,
                               r.estado_reserva_id,
                               estado_reserva = er.estado,
                               r.fecha_retorno,
                               r.hora_retorno,

                           }).ToList();
            if (reserva.Count == 0)
            {
                return NotFound();
            }
            return Ok(reserva);

        }
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult BuscarPorFechaSalida(int filtro)
        {
            reservas? reserva = (from e in _equiposContexto.reservas
                                 where e.equipo_id == filtro
                                 select e).FirstOrDefault();

            if (reserva == null)
            {
                return NotFound();
            }
            return Ok(reserva);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarReserva([FromBody] reservas reserva)
        {

            try
            {
                _equiposContexto.reservas.Add(reserva);
                _equiposContexto.SaveChanges();
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarReserva(int id, [FromBody] reservas reservaModificar)
        {
            reservas? reservaActual = (from e in _equiposContexto.reservas
                                        where e.reserva_id== id
                                        select e).FirstOrDefault();
            if (reservaActual == null)
            {
                return NotFound(id);
            }

            reservaActual.reserva_id = reservaModificar.reserva_id;
            reservaActual.equipo_id = reservaModificar.equipo_id;
            reservaActual.usuario_id = reservaModificar.usuario_id;
            reservaActual.fecha_salida = reservaModificar.fecha_salida;
            reservaActual.hora_salida = reservaModificar.hora_salida;
            reservaActual.tiempo_reserva = reservaModificar.tiempo_reserva;
            reservaActual.estado_reserva_id = reservaModificar.estado_reserva_id;
            reservaActual.fecha_retorno = reservaModificar.fecha_retorno;
            reservaActual.hora_retorno = reservaModificar.hora_retorno;

            _equiposContexto.Entry(reservaActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(reservaModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarReserva(int id)
        {

            reservas? reserva = (from e in _equiposContexto.reservas
                                 where e.reserva_id == id
                                 select e).FirstOrDefault();

            if (reserva == null)
                return NotFound();

            _equiposContexto.reservas.Attach(reserva);
            _equiposContexto.reservas.Remove(reserva);
            _equiposContexto.SaveChanges();

            return Ok(reserva);
        }
    }
}
