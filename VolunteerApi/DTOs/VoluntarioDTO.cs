namespace VolunteerApi.DTOs
{
    public class VoluntarioDTO
    {
        public int VoluntarioId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public DateOnly? FechaNac { get; set; }

        public string Domicilio { get; set; } = string.Empty;

        public string NumeroCelular { get; set; } = string.Empty;

        // La propiedad Especialidad será solo el nombre de la especialidad
        public string Especialidad { get; set; } = string.Empty;

        // La propiedad EspecialidadId agregada para operaciones de ID
        public int? EspecialidadId { get; set; }

        // La propiedad UsuarioId solo se mantiene si necesitas usar el ID del usuario.
        public int? UsuarioId { get; set; }
    }
}
