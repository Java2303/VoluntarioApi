using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolunteerApi.Models;

namespace VolunteerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            var cursos = await _context.Cursos.ToListAsync();
            return Ok(cursos);  // Retorna el resultado con un código 200 OK
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();  // Retorna un código 404 si no se encuentra el curso
            }

            return Ok(curso);  // Devuelve el curso con un código 200 OK
        }

        // PUT: api/Cursos/5
        // Para protegerse de ataques de sobrecarga, es necesario validar que los datos sean correctos
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.CursoId)
            {
                return BadRequest();  // Retorna 400 Bad Request si los IDs no coinciden
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();  // Retorna 404 si el curso no existe
                }
                else
                {
                    throw;  // Propaga el error si ocurre una excepción inesperada
                }
            }

            return NoContent();  // Retorna 204 No Content si la actualización es exitosa
        }

        // POST: api/Cursos
        // Para protegerse de ataques de sobrecarga, es necesario validar los datos de entrada
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Retorna 400 si los datos no son válidos
            }

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurso", new { id = curso.CursoId }, curso);  // Retorna 201 Created con el curso recién creado
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();  // Retorna 404 si no se encuentra el curso
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Curso eliminado exitosamente." });  // Retorna 200 OK con un mensaje de éxito
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.CursoId == id);
        }
    }
}
