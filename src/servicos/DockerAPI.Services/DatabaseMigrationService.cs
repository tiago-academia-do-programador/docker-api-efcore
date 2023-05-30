using Microsoft.AspNetCore.Builder;

namespace DockerAPI.Services
{
    public static class DatabaseMigrationService
    {
        public static void Migrate(IApplicationBuilder app)
        {
            using (var scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                WeatherContext context = services.GetRequiredService<WeatherContext>();

                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }
        }
    }
}