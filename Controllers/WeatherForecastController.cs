using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

namespace AppConfigTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AzureFeatureFlagProvider _featureFlagProvider; // Inject the IFeatureFlagProvider
        private readonly ExampleTargetingContextAccessor _contextAccessor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFeatureManager featureManager, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _contextAccessor = new ExampleTargetingContextAccessor(httpContext);
            _featureFlagProvider = new AzureFeatureFlagProvider(featureManager, _contextAccessor);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var showSummary = await _featureFlagProvider.IsEnabled("ShowSummary"); // Check if the feature flag is enabled

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = showSummary ? Summaries[Random.Shared.Next(Summaries.Length)] : null // Only set the Summary if the feature flag is enabled
            })
            .ToArray();
        }
    }
}
