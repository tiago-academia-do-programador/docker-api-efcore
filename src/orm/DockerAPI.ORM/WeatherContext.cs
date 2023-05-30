using DockerAPI.Dominio;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DockerAPI.ORM
{
    public class WeatherContext: DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options) { }

        public DbSet<WeatherForecast> Forecasts { get; set; } = null;
    }
}