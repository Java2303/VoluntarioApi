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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();
            return usuario;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
                return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] UsuarioCreateDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                Email = usuarioDTO.Email,
                Contraseña = usuarioDTO.Contraseña,
                RolId = usuarioDTO.RolId ?? 2,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioId }, usuario);
        }

        [HttpPost("registrar/usuarios")]
        public async Task<IActionResult> RegistrarVols([FromBody] RegistroUsuarioVoluntarioRequest request)
        {
            var usuarioDTO = request.Usuario;
            var voluntarioDTO = request.Voluntario;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                Email = usuarioDTO.Email,
                Contraseña = usuarioDTO.Contraseña,
                RolId = usuarioDTO.RolId ?? 2,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            if (usuario.RolId == 3 && voluntarioDTO != null)
            {
                var especialidadExiste = await _context.Especialidades.AnyAsync(e => e.EspecialidadId == voluntarioDTO.EspecialidadId);
                if (!especialidadExiste)
                    return BadRequest("La especialidad no existe.");

                var voluntario = new Voluntario
                {
                    UsuarioId = usuario.UsuarioId,
                    Sexo = voluntarioDTO.Sexo ?? "O",
                    FechaNac = voluntarioDTO.FechaNac,
                    Domicilio = voluntarioDTO.Domicilio ?? "",
                    NumeroCelular = voluntarioDTO.NumeroCelular ?? "",
                    EspecialidadId = voluntarioDTO.EspecialidadId
                };

                _context.Voluntarios.Add(voluntario);
                await _context.SaveChangesAsync();
            }

            return Ok(new { mensaje = "Usuario y voluntario registrados correctamente" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Email) || string.IsNullOrWhiteSpace(loginDTO.Contraseña))
                return BadRequest(new { mensaje = "Email y contraseña son obligatorios" });

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (usuario == null)
                return Unauthorized(new { mensaje = "Usuario no encontrado" });

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

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
