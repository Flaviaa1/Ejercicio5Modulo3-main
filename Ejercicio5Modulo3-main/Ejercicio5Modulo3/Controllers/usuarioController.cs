using Ejercicio5Modulo3.Domain.Entities;
using Ejercicio5Modulo3.Servicios.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ejercicio5Modulo3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class usuarioController : ControllerBase
    {
        public Iservicios _IServicios;

        public usuarioController(Iservicios iservicios)
        {
            _IServicios = iservicios;

        }

        //HttpClient
        //HttpClientFactory
        public class PostcodeConverter : JsonConverter<string>
        {
            public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    return reader.GetString();
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.GetInt32().ToString();
                }
                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value);
            }
        }
        [HttpPost("api/v1/usuarios/seed")]
        public async Task<IActionResult> GuardarUsuariosDesdeApi()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://randomuser.me/");
            var resultado = await client.GetAsync("api/?results=500");
            var content = await resultado.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new PostcodeConverter() }
            });

            var usuarios = apiResponse.Results.Select(u => new Usuario
            {
                Nombre = u.Name.First,
                Apellido = u.Name.Last,
                Edad = u.Dob.Age,
                Genero = u.Gender,
                Email = u.Email,
                NombreUsuario = u.Login.Username,
                Password = u.Login.Password,
                Ciudad = u.Location.City,
                Estado = u.Location.State,
                Pais = u.Location.Country
            }).ToList();

            var mensaje = await _IServicios.GuardarUsuariosAsync(usuarios);

            return Ok(mensaje);
        }

        [HttpGet("api/v1/usuarios")]
        public async Task<IActionResult> ObtenerUsuarios([FromQuery] int? edad, [FromQuery] string? sexo)
        {
            List<Usuario> usuarios;

            if (edad.HasValue && !string.IsNullOrWhiteSpace(sexo))
            {
                // Obtener usuarios con edad y sexo específicos
                usuarios = await _IServicios.ObtenerUsuariosPorEdadYSexoAsync(edad.Value, sexo);
            }
            else
            {
                // Obtener todos los usuarios si no se especifica edad o sexo
                usuarios = await _IServicios.ObtenerTodosUsuariosAsync();
            }

            return Ok(usuarios);
        }

    }
    }
