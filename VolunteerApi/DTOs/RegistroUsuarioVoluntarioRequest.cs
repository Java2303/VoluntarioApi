namespace VolunteerApi.DTOs
{
    public class RegistroUsuarioVoluntarioRequest
    {
        public UsuarioCreateDTO Usuario { get; set; } = new UsuarioCreateDTO();
        public VoluntarioDTO? Voluntario { get; set; } // Solo requerido si el rol es 3
    }
}
