using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public facultadesController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<facultades> listadoFacultad = (from e in _equiposContexto.facultades
                                             select e).ToList();

            if (listadoFacultad.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoFacultad);

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            facultades? facultad = (from e in _equiposContexto.facultades
                                 where e.facultad_id == id
                                 select e).FirstOrDefault();

            if (facultad == null)
            {
                return NotFound();
            }
            return Ok(facultad);

        }
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult BuscarPorNombre(String filtro)
        {
            facultades? facultad = (from e in _equiposContexto.facultades
                                 where e.nombre_facultad.Contains(filtro)
                                 select e).FirstOrDefault();

            if (facultad == null)
            {
                return NotFound();
            }
            return Ok(facultad);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarFacultad([FromBody] facultades facultad)
        {

            try
            {
                _equiposContexto.facultades.Add(facultad);
                _equiposContexto.SaveChanges();
                return Ok(facultad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarFacultad(int id, [FromBody] facultades facultadModificar)
        {
            facultades? facultadActual = (from e in _equiposContexto.facultades
                                        where e.facultad_id == id
                                        select e).FirstOrDefault();
            if (facultadActual == null)
            {
                return NotFound(id);
            }

            facultadActual.facultad_id = facultadModificar.facultad_id;
            facultadActual.nombre_facultad = facultadModificar.nombre_facultad;
   

            _equiposContexto.Entry(facultadActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(facultadModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarFacultad(int id)
        {

            facultades? facultad = (from e in _equiposContexto.facultades
                                 where e.facultad_id == id
                                 select e).FirstOrDefault();

            if (facultad == null)
                return NotFound();

            _equiposContexto.facultades.Attach(facultad);
            _equiposContexto.facultades.Remove(facultad);
            _equiposContexto.SaveChanges();

            return Ok(facultad);
        }
    }
}
