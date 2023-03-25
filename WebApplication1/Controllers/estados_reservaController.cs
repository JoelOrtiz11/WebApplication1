using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_reservaController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estados_reservaController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<estados_reserva> listadoEstado = (from e in _equiposContexto.estados_reserva
                                             select e).ToList();

            if (listadoEstado.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoEstado);

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            estados_reserva? estado = (from e in _equiposContexto.estados_reserva
                                 where e.estado_res_id == id
                                 select e).FirstOrDefault();

            if (estado == null)
            {
                return NotFound();
            }
            return Ok(estado);

        }
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult BuscarPorEstado(String filtro)
        {
            estados_reserva? estado = (from e in _equiposContexto.estados_reserva
                                 where e.estado.Contains(filtro)
                                 select e).FirstOrDefault();

            if (estado == null)
            {
                return NotFound();
            }
            return Ok(estado);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEstado([FromBody] estados_reserva estado)
        {

            try
            {
                _equiposContexto.estados_reserva.Add(estado);
                _equiposContexto.SaveChanges();
                return Ok(estado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarEstado(int id, [FromBody] estados_reserva estadoModificar)
        {
            estados_reserva? estadoActual = (from e in _equiposContexto.estados_reserva
                                        where e.estado_res_id == id
                                        select e).FirstOrDefault();
            if (estadoActual == null)
            {
                return NotFound(id);
            }

            estadoActual.estado_res_id = estadoModificar.estado_res_id;
            estadoActual.estado = estadoModificar.estado;
          
            _equiposContexto.Entry(estadoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(estadoModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEstado(int id)
        {

            estados_reserva? estado = (from e in _equiposContexto.estados_reserva
                                 where e.estado_res_id == id
                                 select e).FirstOrDefault();

            if (estado == null)
                return NotFound();

            _equiposContexto.estados_reserva.Attach(estado);
            _equiposContexto.estados_reserva.Remove(estado);
            _equiposContexto.SaveChanges();

            return Ok(estado);
        }
    }
}
