namespace VolunteerApi.DTOs
{
    public class CursoDTO
    {
        public int CursoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public string? Categoria { get; set; }
        public string? Dificultad { get; set; }
        public string? ImagenUrl { get; set; }
    }
}
