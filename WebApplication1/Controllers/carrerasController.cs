using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public carrerasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<carreras> listadoCarrera = (from e in _equiposContexto.carreras
                                           select e).ToList();

            if (listadoCarrera.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoCarrera);

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            var carrera = (from c in _equiposContexto.carreras
                            join f in _equiposContexto.facultades on c.facultad_id equals f.facultad_id
                            select new
                            {
                                c.carrera_id,
                                c.nombre_carrera,
                                c.facultad_id,
                                f.nombre_facultad,

                            }).ToList();
            if (carrera.Count == 0)
            {
                return NotFound();
            }
            return Ok(carrera);

        }
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult BuscarPorNombre(String filtro)
        {
            carreras? carrera = (from e in _equiposContexto.carreras
                               where e.nombre_carrera.Contains(filtro)
                               select e).FirstOrDefault();

            if (carrera == null)
            {
                return NotFound();
            }
            return Ok(carrera);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarCarrera([FromBody] carreras carrera)
        {

            try
            {
                _equiposContexto.carreras.Add(carrera);
                _equiposContexto.SaveChanges();
                return Ok(carrera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarCarrera(int id, [FromBody] carreras carreraModificar)
        {
            carreras? carrerasActual = (from e in _equiposContexto.carreras
                                      where e.carrera_id == id
                                      select e).FirstOrDefault();
            if (carrerasActual == null)
            {
                return NotFound(id);
            }

            carrerasActual.carrera_id = carreraModificar.carrera_id;
            carrerasActual.nombre_carrera = carreraModificar.nombre_carrera;
            carrerasActual.facultad_id = carreraModificar.facultad_id;

            _equiposContexto.Entry(carrerasActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(carreraModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarCarrera(int id)
        {

            carreras? carrera = (from e in _equiposContexto.carreras
                               where e.carrera_id == id
                               select e).FirstOrDefault();

            if (carrera == null)
                return NotFound();

            _equiposContexto.carreras.Attach(carrera);
            _equiposContexto.carreras.Remove(carrera);
            _equiposContexto.SaveChanges();

            return Ok(carrera);
        }
    }
}
