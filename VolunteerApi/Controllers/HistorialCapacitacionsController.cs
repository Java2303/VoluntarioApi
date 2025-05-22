using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolunteerApi.Models;

namespace VolunteerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialCapacitacionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistorialCapacitacionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/HistorialCapacitacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialCapacitacion>>> GetHistorialCapacitacions()
        {
            return await _context.HistorialCapacitacions.ToListAsync();
        }

        // GET: api/HistorialCapacitacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistorialCapacitacion>> GetHistorialCapacitacion(int id)
        {
            var historialCapacitacion = await _context.HistorialCapacitacions.FindAsync(id);

            if (historialCapacitacion == null)
            {
                return NotFound();
            }

            return historialCapacitacion;
        }

        // PUT: api/HistorialCapacitacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorialCapacitacion(int id, HistorialCapacitacion historialCapacitacion)
        {
            if (id != historialCapacitacion.HistorialId)
            {
                return BadRequest();
            }

            _context.Entry(historialCapacitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorialCapacitacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HistorialCapacitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HistorialCapacitacion>> PostHistorialCapacitacion(HistorialCapacitacion historialCapacitacion)
        {
            _context.HistorialCapacitacions.Add(historialCapacitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistorialCapacitacion", new { id = historialCapacitacion.HistorialId }, historialCapacitacion);
        }

        // DELETE: api/HistorialCapacitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorialCapacitacion(int id)
        {
            var historialCapacitacion = await _context.HistorialCapacitacions.FindAsync(id);
            if (historialCapacitacion == null)
            {
                return NotFound();
            }

            _context.HistorialCapacitacions.Remove(historialCapacitacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistorialCapacitacionExists(int id)
        {
            return _context.HistorialCapacitacions.Any(e => e.HistorialId == id);
        }
    }
}
