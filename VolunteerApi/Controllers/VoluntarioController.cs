using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolunteerApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolunteerApi.DTOs;

namespace VolunteerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoluntarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VoluntarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Voluntario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoluntarioDTO>>> GetVoluntarios()
        {
            var voluntarios = await _context.Voluntarios
                .Include(v => v.Usuario) // Incluye los datos de Usuario
                .Include(v => v.Especialidad) // Incluye los datos de Especialidad
                .ToListAsync();

            var voluntariosDTO = voluntarios.Select(v => new VoluntarioDTO
            {
                VoluntarioId = v.VoluntarioId,
                Nombre = v.Usuario?.Nombre ?? "Sin nombre",
                Apellido = v.Usuario?.Apellido ?? "Sin apellido",
                Sexo = v.Sexo ?? "No especificado",
                FechaNac = v.FechaNac,
                Domicilio = v.Domicilio ?? "No especificado",
                NumeroCelular = v.NumeroCelular ?? "No especificado",
                Especialidad = v.Especialidad?.NombreEspecialidad ?? "Sin especialidad"
            });

            return Ok(voluntariosDTO);
        }

        // GET: api/Voluntario/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<VoluntarioDTO>> GetVoluntario(int id)
        {
            var voluntario = await _context.Voluntarios
                .Include(v => v.Usuario)
                .Include(v => v.Especialidad)
                .FirstOrDefaultAsync(v => v.VoluntarioId == id);

            if (voluntario == null)
                return NotFound();

            var voluntarioDTO = new VoluntarioDTO
            {
                VoluntarioId = voluntario.VoluntarioId,
                Nombre = voluntario.Usuario?.Nombre ?? "Sin nombre",
                Apellido = voluntario.Usuario?.Apellido ?? "Sin apellido",
                Sexo = voluntario.Sexo ?? "No especificado",
                FechaNac = voluntario.FechaNac,
                Domicilio = voluntario.Domicilio ?? "No especificado",
                NumeroCelular = voluntario.NumeroCelular ?? "No especificado",
                Especialidad = voluntario.Especialidad?.NombreEspecialidad ?? "Sin especialidad"
            };

            return Ok(voluntarioDTO);
        }




        // POST: api/Voluntario
        [HttpPost]
        public async Task<ActionResult<VoluntarioDTO>> PostVoluntario([FromBody] VoluntarioDTO voluntarioDTO)
        {
            // Verificar si el UsuarioId es válido
            var usuario = await _context.Usuarios.FindAsync(voluntarioDTO.UsuarioId);
            if (usuario == null)
            {
                return BadRequest("El usuario no existe.");
            }

            // Verificar si el EspecialidadId es válido
            var especialidad = await _context.Especialidades.FindAsync(voluntarioDTO.EspecialidadId);
            if (especialidad == null)
            {
                return BadRequest("La especialidad no existe.");
            }

            // Crear un nuevo Voluntario
            var nuevoVoluntario = new Voluntario
            {
                UsuarioId = voluntarioDTO.UsuarioId,  // Asignar UsuarioId
                EspecialidadId = voluntarioDTO.EspecialidadId,  // Asignar EspecialidadId
                Sexo = voluntarioDTO.Sexo,
                FechaNac = voluntarioDTO.FechaNac,
                Domicilio = voluntarioDTO.Domicilio,
                NumeroCelular = voluntarioDTO.NumeroCelular
            };

            // Agregar el nuevo voluntario a la base de datos
            _context.Voluntarios.Add(nuevoVoluntario);

            try
            {
                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Devolver el DTO del voluntario recién creado
                return CreatedAtAction(nameof(GetVoluntario), new { id = nuevoVoluntario.VoluntarioId }, voluntarioDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al guardar el voluntario: " + ex.Message);
            }
        }
    }
}
