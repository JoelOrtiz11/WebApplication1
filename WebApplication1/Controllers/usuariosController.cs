using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public usuariosController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<usuarios> listadoUsuario = (from e in _equiposContexto.usuarios
                                             select e).ToList();

            if (listadoUsuario.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoUsuario);

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            var usuario = (from u in _equiposContexto.usuarios
                            join c in _equiposContexto.carreras on u.carrera_id equals c.carrera_id
                            select new
                            {
                                u.usuario_id,
                                u.nombre,
                                u.documento,
                                u.tipo,
                                u.carnet,
                                u.carrera_id,
                                c.nombre_carrera,

                            }).ToList();
            if (usuario.Count == 0)
            {
                return NotFound();
            }
            return Ok(usuario);

        }
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult BuscarPorNombre(String filtro)
        {
            usuarios? usuario = (from e in _equiposContexto.usuarios
                                 where e.nombre.Contains(filtro)
                                 select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarUsuario([FromBody] usuarios usuario)
        {

            try
            {
                _equiposContexto.usuarios.Add(usuario);
                _equiposContexto.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarUsuario(int id, [FromBody] usuarios usuarioModificar)
        {
            usuarios? usuarioActual = (from e in _equiposContexto.usuarios
                                        where e.usuario_id == id
                                        select e).FirstOrDefault();
            if (usuarioActual == null)
            {
                return NotFound(id);
            }

            usuarioActual.usuario_id = usuarioModificar.usuario_id;
            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.documento = usuarioModificar.documento;
            usuarioActual.tipo = usuarioModificar.tipo;
            usuarioActual.carnet = usuarioModificar.carnet;
            usuarioActual.carrera_id = usuarioModificar.carrera_id;

            _equiposContexto.Entry(usuarioActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(usuarioModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarUsuario(int id)
        {

            usuarios? usuario = (from e in _equiposContexto.usuarios
                                 where e.usuario_id == id
                                 select e).FirstOrDefault();

            if (usuario == null)
                return NotFound();

            _equiposContexto.usuarios.Attach(usuario);
            _equiposContexto.usuarios.Remove(usuario);
            _equiposContexto.SaveChanges();

            return Ok(usuario);
        }
    }
}
