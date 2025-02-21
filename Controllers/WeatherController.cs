using HybridWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HybridWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly HybridCache _cache;

        public WeatherController(HybridCache cache)
        {
            _cache = cache;
        }

        [HttpGet("{city}")]
        public async Task<ActionResult<WeatherForecast>> GetWeather(string city)
        {
            var cacheKey = $"weather_{city}";

            // Try to get the cached data
            var weather = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                // Simulate fetching data from an external API
                var forecast = await FetchWeatherFromApi(city);

                // Set cache expiration
                forecast.Tims = DateTime.Now;

                return forecast;
            });

            return Ok(weather);
        }

        private async Task<WeatherForecast> FetchWeatherFromApi(string city)
        {
            // Simulate an API call
            await Task.Delay(100); // Simulate network latency
            return new WeatherForecast
            {
                City = city,
                Temperature = Random.Shared.Next(-20, 55),
                Summary = "Sunny"
            };
        }
    }
}
