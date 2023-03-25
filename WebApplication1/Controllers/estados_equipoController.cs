using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estados_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<estados_equipo> listadoEstados = (from e in _equiposContexto.estados_equipo
                                             select e).ToList();

            if (listadoEstados.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoEstados);

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            estados_equipo? estado = (from e in _equiposContexto.estados_equipo
                                 where e.id_estados_equipo == id
                                 select e).FirstOrDefault();

            if (estado == null)
            {
                return NotFound();
            }
            return Ok(estado);

        }
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult BuscarPorDescripcion(String filtro)
        {
            estados_equipo? estado = (from e in _equiposContexto.estados_equipo
                                 where e.descripcion.Contains(filtro)
                                 select e).FirstOrDefault();

            if (estado == null)
            {
                return NotFound();
            }
            return Ok(estado);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEstado([FromBody] estados_equipo estado)
        {

            try
            {
                _equiposContexto.estados_equipo.Add(estado);
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

        public IActionResult ActualizarCarrera(int id, [FromBody] estados_equipo estadoModificar)
        {
            estados_equipo? estadoActual = (from e in _equiposContexto.estados_equipo
                                        where e.id_estados_equipo == id
                                        select e).FirstOrDefault();
            if (estadoActual == null)
            {
                return NotFound(id);
            }

            estadoActual.id_estados_equipo = estadoModificar.id_estados_equipo;
            estadoActual.descripcion = estadoModificar.descripcion;
            estadoActual.estado = estadoModificar.estado;

            _equiposContexto.Entry(estadoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(estadoModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEstado(int id)
        {

            estados_equipo? estado = (from e in _equiposContexto.estados_equipo
                                 where e.id_estados_equipo == id
                                 select e).FirstOrDefault();

            if (estado == null)
                return NotFound();

            _equiposContexto.estados_equipo.Attach(estado);
            _equiposContexto.estados_equipo.Remove(estado);
            _equiposContexto.SaveChanges();

            return Ok(estado);
        }
    }
}
