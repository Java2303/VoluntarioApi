namespace VolunteerApi.DTOs;

public class CursoDto
{
    public int CursoId { get; set; }
    public string Nombre { get; set; } = null!;
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public string? Categoria { get; set; }
    public string? Dificultad { get; set; }
    public string? ImagenUrl { get; set; }
}
