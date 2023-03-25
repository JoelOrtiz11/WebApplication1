using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public tipo_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<tipo_equipo> listadoTipo = (from e in _equiposContexto.tipo_equipo
                                             select e).ToList();

            if (listadoTipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoTipo);

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            tipo_equipo? tipo = (from e in _equiposContexto.tipo_equipo
                                 where e.id_tipo_equipo == id
                                 select e).FirstOrDefault();

            if (tipo == null)
            {
                return NotFound();
            }
            return Ok(tipo);

        }
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult BuscarPorDescripcion(String filtro)
        {
            tipo_equipo? tipo = (from e in _equiposContexto.tipo_equipo
                                 where e.descripcion.Contains(filtro)
                                 select e).FirstOrDefault();

            if (tipo == null)
            {
                return NotFound();
            }
            return Ok(tipo);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarTipo([FromBody] tipo_equipo tipo)
        {

            try
            {
                _equiposContexto.tipo_equipo.Add(tipo);
                _equiposContexto.SaveChanges();
                return Ok(tipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarTipo(int id, [FromBody] tipo_equipo tipoModificar)
        {
            tipo_equipo? tipoActual = (from e in _equiposContexto.tipo_equipo where e.id_tipo_equipo == id
                                        select e).FirstOrDefault();
            if (tipoActual == null)
            {
                return NotFound(id);
            }

            tipoActual.id_tipo_equipo = tipoModificar.id_tipo_equipo;
            tipoActual.descripcion = tipoModificar.descripcion;
            tipoActual.estado = tipoModificar.estado;

            _equiposContexto.Entry(tipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(tipoModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarTipo(int id)
        {

            tipo_equipo? tipo = (from e in _equiposContexto.tipo_equipo
                                 where e.id_tipo_equipo == id
                                 select e).FirstOrDefault();

            if (tipo == null)
                return NotFound();

            _equiposContexto.tipo_equipo.Attach(tipo);
            _equiposContexto.tipo_equipo.Remove(tipo);
            _equiposContexto.SaveChanges();

            return Ok(tipo);
        }
    }
}
