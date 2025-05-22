namespace VolunteerApi.DTOs
{
    public class EventoDTO
    {
        public int EventoId { get; set; }
        public string NombreEvento { get; set; } = string.Empty;
        public DateOnly Fecha { get; set; }
        public string? Ubicacion { get; set; }
        public string? Descripcion { get; set; }

        // Datos básicos del organizador
        public UsuarioCreateDTO? Organizador { get; set; }

        // Lista de voluntarios asociados (datos resumidos)
        public List<VoluntarioDTO> Voluntarios { get; set; } = new();
    }
}
