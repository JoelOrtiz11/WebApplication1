using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public marcasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<marcas> listadoMarca = (from e in _equiposContexto.marcas
                                             select e).ToList();

            if (listadoMarca.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoMarca);

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            marcas? marca = (from e in _equiposContexto.marcas
                                 where e.id_marcas == id
                                 select e).FirstOrDefault();

            if (marca == null)
            {
                return NotFound();
            }
            return Ok(marca);

        }
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult BuscarPorNombre(String filtro)
        {
            marcas? marca = (from e in _equiposContexto.marcas
                                 where e.nombre_marca.Contains(filtro)
                                 select e).FirstOrDefault();

            if (marca == null)
            {
                return NotFound();
            }
            return Ok(marca);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarMarca([FromBody] marcas marca)
        {

            try
            {
                _equiposContexto.marcas.Add(marca);
                _equiposContexto.SaveChanges();
                return Ok(marca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarMarca(int id, [FromBody] marcas marcaModificar)
        {
            marcas? marcaActual = (from e in _equiposContexto.marcas
                                        where e.id_marcas == id
                                        select e).FirstOrDefault();
            if (marcaActual == null)
            {
                return NotFound(id);
            }

            marcaActual.id_marcas = marcaModificar.id_marcas;
            marcaActual.nombre_marca = marcaModificar.nombre_marca;
            marcaActual.estados = marcaModificar.estados;

            _equiposContexto.Entry(marcaActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(marcaModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarMarca(int id)
        {

            marcas? marca = (from e in _equiposContexto.marcas
                                 where e.id_marcas == id
                                 select e).FirstOrDefault();

            if (marca == null)
                return NotFound();

            _equiposContexto.marcas.Attach(marca);
            _equiposContexto.marcas.Remove(marca);
            _equiposContexto.SaveChanges();

            return Ok(marca);
        }
    }
}
