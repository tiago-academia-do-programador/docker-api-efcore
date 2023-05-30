using DockerAPI.ORM;
using Microsoft.EntityFrameworkCore;


namespace DockerAPI.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<WeatherContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                opt => opt.EnableRetryOnFailure()));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            ILogger logger = app.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                MigrateDatabase(app, logger);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Database: An error occurred while migrating the database.");
            }

            app.Run();
        }

        private static void MigrateDatabase(WebApplication app, ILogger logger)
        {
            using (var scope = app.Services.CreateScope())
            {
                logger.LogInformation("[DB]: Iniciando processo de migração do banco de dados...");

                IServiceProvider services = scope.ServiceProvider;

                WeatherContext context = services.GetRequiredService<WeatherContext>();

                if (!context.Database.CanConnect())
                {
                    logger.LogInformation("[DB]: O servidor do banco de dados está inativo... esperando por 30 segundos.");

                    Thread.Sleep(30000);
                }

                logger.LogInformation("[DB]: Banco de dados criado. Procurando por migrações...");

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();

                    logger.LogInformation("[DB]: Migrações completas.");
                }
            }
        }
    }
}