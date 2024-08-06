using Ejercicio5Modulo3.Repository;
using Ejercicio5Modulo3.Servicios.interfaces;
using Ejercicio5Modulo3.Servicios;
using Microsoft.EntityFrameworkCore;

namespace Ejercicio5Modulo3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpClient<servicio>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var cone = builder.Configuration.GetConnectionString("conexion");
            builder.Services.AddDbContext<Ejercicio5Modulo3Context>(opt => { opt.UseSqlServer(cone); });
            builder.Services.AddScoped<Iservicios, servicio>();
             var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}