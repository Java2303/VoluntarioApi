namespace VolunteerApi.DTOs
{
    public class EventoCreateDTO
    {
        public string NombreEvento { get; set; } = string.Empty;
        public DateOnly Fecha { get; set; }
        public string? Ubicacion { get; set; }
        public string? Descripcion { get; set; }

        // Solo se necesita el ID del usuario que organiza
        public int OrganizadorId { get; set; }
    }


}
