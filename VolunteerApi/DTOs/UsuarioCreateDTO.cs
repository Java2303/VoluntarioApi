using System.ComponentModel.DataAnnotations;

namespace VolunteerApi.DTOs
{
    public class UsuarioCreateDTO

    {
        public int UsuarioId { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;
        [Required]
        public string? Apellido { get; set; } 
        [Required]
        public string? Email { get; set; } 
        [Required]
        public string? Contraseña { get; set; }
        public int? RolId { get; set; } 
    }
}
