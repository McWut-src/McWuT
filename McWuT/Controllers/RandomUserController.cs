using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace McWuT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RandomUserController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RandomUserController> _logger;

        public RandomUserController(IHttpClientFactory httpClientFactory, ILogger<RandomUserController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRandomUsers(
            [FromQuery] int results = 5,
            [FromQuery] string? gender = null,
            [FromQuery] string? nat = null)
        {
            try
            {
                // Validate parameters
                if (results < 1 || results > 100)
                {
                    return BadRequest("Results must be between 1 and 100");
                }

                // Build the RandomUser API URL
                var url = $"https://randomuser.me/api/?results={results}";
                if (!string.IsNullOrEmpty(gender))
                {
                    url += $"&gender={gender}";
                }
                if (!string.IsNullOrEmpty(nat))
                {
                    url += $"&nat={nat}";
                }

                _logger.LogInformation("Requesting random users from: {Url}", url);

                // Make the request to RandomUser API
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                
                // Parse and return the JSON response
                var jsonDocument = JsonDocument.Parse(content);
                return Ok(jsonDocument.RootElement);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error making request to RandomUser API");
                return StatusCode(500, "Error fetching data from RandomUser API");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing response from RandomUser API");
                return StatusCode(500, "Error parsing response from RandomUser API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in RandomUser API");
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}