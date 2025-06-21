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
            return Ok(cursos);
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
                return NotFound();

            return Ok(curso);
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.CursoId)
                return BadRequest();

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurso", new { id = curso.CursoId }, curso);
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound();

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Curso eliminado exitosamente." });
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.CursoId == id);
        }
    }
}
