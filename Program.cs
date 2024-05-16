using Microsoft.FeatureManagement;

namespace AppConfigTestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string connectionString = builder.Configuration.GetConnectionString("AppConfig")!;

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Load configuration from Azure App Configuration
            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                // Connect and Load all feature flags with no label
                options
                    .Connect(connectionString)          // To connect to App Configuration
                    .UseFeatureFlags(featureOptions =>  // To load feature flags
                    {
                        // Flags are cached for 45 seconds
                        featureOptions.CacheExpirationInterval = new TimeSpan(0, 0, 45);
                    });
            });

            builder.Services.AddAzureAppConfiguration();

            builder.Services.AddFeatureManagement();

            // Add HttpContextAccessor to the container of services.
            builder.Services.AddHttpContextAccessor();

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

            app.UseAzureAppConfiguration();

            app.MapControllers();

            app.Run();
        }
    }
}
