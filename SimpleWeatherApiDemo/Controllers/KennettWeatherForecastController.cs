using Microsoft.AspNetCore.Mvc;
using SimpleWeatherApiDemo.ExternalServices;
using SimpleWeatherApiDemo.Interfaces;

namespace SimpleWeatherApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KennettWeatherForecastController(WeatherDotGovClient apiClient, 
        IWeatherCacheService weatherCacheService) : ControllerBase
    {
        [HttpGet(Name = "GetKennettWeatherForecast")]
        [ProducesResponseType<WeatherResponse>(200)]
        [ProducesResponseType<ProblemDetails>(500)]
        public async Task<IActionResult> GetKennetWeather()
        {
            var kennetCoordinates = $"32,68";
            try
            {
                var cachedResult = weatherCacheService.Get(kennetCoordinates);
                if (cachedResult is WeatherResponse weatherResponse)
                {
                    return Ok(weatherResponse);
                }
            }
            catch (Exception ex) 
            {
                //maybe would log the cache specifics and return a more generic message to the user
                return StatusCode(500, "Error accessing cache");
            }

            try
            {
                var apiResult = await apiClient.GetWeatherForecastAsync(kennetCoordinates);
                if (apiResult.HasValue)
                {
                    weatherCacheService.Add(kennetCoordinates, apiResult.Value!, DateTime.UtcNow.AddMinutes(1));
                    return Ok(apiResult.Value!);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                //maybe would log the http call specifics and return a more generic message to the user
                return StatusCode(500, "Error accessing NWS API");
            }
        }
    }
}
