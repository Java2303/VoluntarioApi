using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolunteerApi.DTOs;
using VolunteerApi.Models;

namespace VolunteerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] UsuarioCreateDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Contraseña sin hash (solo para desarrollo)
            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                Email = usuarioDTO.Email,
                Contraseña = usuarioDTO.Contraseña, // sin hash
                RolId = usuarioDTO.RolId ?? 2,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioId }, usuario);
        }

        // POST: api/Usuarios/registrar/usuarios
        [HttpPost("registrar/usuarios")]
        public async Task<IActionResult> RegistrarVols([FromBody] RegistroUsuarioVoluntarioRequest request)
        {
            var usuarioDTO = request.Usuario;
            var voluntarioDTO = request.Voluntario;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                Email = usuarioDTO.Email,
                Contraseña = usuarioDTO.Contraseña, // sin hash
                RolId = usuarioDTO.RolId ?? 2,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            if (usuario.RolId == 3 && voluntarioDTO != null)
            {
                var especialidad = await _context.Especialidades
                    .FirstOrDefaultAsync(e => e.NombreEspecialidad == voluntarioDTO.Especialidad);

                var voluntario = new Voluntario
                {
                    UsuarioId = usuario.UsuarioId,
                    Sexo = voluntarioDTO.Sexo ?? "O",
                    FechaNac = voluntarioDTO.FechaNac,
                    Domicilio = voluntarioDTO.Domicilio ?? "",
                    NumeroCelular = voluntarioDTO.NumeroCelular ?? "",
                    EspecialidadId = especialidad?.EspecialidadId
                };

                _context.Voluntarios.Add(voluntario);
                await _context.SaveChangesAsync();
            }

            return Ok(new { mensaje = "Usuario y voluntario registrados correctamente" });
        }

        // POST: api/Usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Email) || string.IsNullOrWhiteSpace(loginDTO.Contraseña))
                return BadRequest(new { mensaje = "Email y contraseña son obligatorios" });

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Usuario no encontrado" });

            // Comparar contraseña directamente (sin hash)
            if (loginDTO.Contraseña != usuario.Contraseña)
                return Unauthorized(new { mensaje = "Contraseña incorrecta" });

            return Ok(new
            {
                usuario = new
                {
                    usuario.UsuarioId,
                    usuario.Nombre,
                    usuario.Email
                }
            });
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
