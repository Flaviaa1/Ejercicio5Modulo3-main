using Ejercicio5Modulo3.Domain.Entities;
using Ejercicio5Modulo3.Repository;
using Ejercicio5Modulo3.Servicios.interfaces;
using static Ejercicio5Modulo3.Controllers.usuarioController;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Ejercicio5Modulo3.Servicios
{
    public class servicio:Iservicios
    {
        private readonly Ejercicio5Modulo3Context _context;
        private readonly HttpClient _httpClient;

        public servicio(Ejercicio5Modulo3Context contexto, HttpClient httpClient)
        {
            _context = contexto;
            _httpClient = httpClient;
        }

        

        public async Task<List<OriginalUsuario>> GetProvedorAsync()
        {
            var response = await _httpClient.GetAsync("https://randomuser.me/api/?results=500");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new PostcodeConverter() }
            });

            return apiResponse.Results;
        }

        public async Task<string> GuardarUsuariosAsync(List<Usuario> usuarios)
        {
            // emails de los usuarios ya existentes en la base de datos
            var emailsExistentes = await _context.Usuario
                .Select(u => u.Email)
                .ToListAsync();

            //  usuarios que no están en la base de datos
            var usuariosParaAgregar = usuarios
                .Where(u => !emailsExistentes.Contains(u.Email))
                .ToList();

            if (usuariosParaAgregar.Any())
            {
                _context.Usuario.AddRange(usuariosParaAgregar);
                await _context.SaveChangesAsync();
            }

            // Mensaje de respuesta
            var mensaje = $"Se agregaron {usuariosParaAgregar.Count} usuarios.";

            return mensaje;
        }




        public async Task<List<Usuario>> ObtenerUsuariosPorEdadYSexoAsync(int edad, string genero)
        {
            // Convertir genero a minúsculas
            string generoLower = genero?.ToLower();

            return await _context.Usuario
                .Where(u => u.Edad == edad && u.Genero.ToLower() == generoLower)
                .ToListAsync();
        }

        public async Task<List<Usuario>> ObtenerTodosUsuariosAsync()
        {
            return await _context.Usuario.ToListAsync();
        }
    }


}

