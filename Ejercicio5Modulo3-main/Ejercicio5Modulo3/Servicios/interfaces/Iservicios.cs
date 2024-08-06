using Ejercicio5Modulo3.Domain.Entities;

namespace Ejercicio5Modulo3.Servicios.interfaces
{
    public interface Iservicios
    {
        Task<List<OriginalUsuario>> GetProvedorAsync();
        Task<string> GuardarUsuariosAsync(List<Usuario> usuarios);
        Task<List<Usuario>> ObtenerUsuariosPorEdadYSexoAsync(int edad, string sexo);
        Task<List<Usuario>> ObtenerTodosUsuariosAsync();
    }
}
