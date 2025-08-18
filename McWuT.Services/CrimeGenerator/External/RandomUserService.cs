using McWuT.Common.CrimeGenerator.ExternalApis;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace McWuT.Services.CrimeGenerator.External;

public class RandomUserService : IRandomUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RandomUserService> _logger;
    private const string BaseUrl = "https://randomuser.me/api/";

    public RandomUserService(HttpClient httpClient, ILogger<RandomUserService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<RandomUserResult[]> GetRandomUsersAsync(int count = 1, string? gender = null, string? nationality = null)
    {
        try
        {
            var queryParams = new List<string> { $"results={count}" };
            
            if (!string.IsNullOrEmpty(gender))
                queryParams.Add($"gender={gender.ToLower()}");
            
            if (!string.IsNullOrEmpty(nationality))
                queryParams.Add($"nat={nationality.ToUpper()}");

            var url = $"{BaseUrl}?{string.Join("&", queryParams)}";
            
            _logger.LogInformation("Fetching {Count} random users from RandomUser API", count);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var randomUserResponse = JsonSerializer.Deserialize<RandomUserResponse>(json, options);
            
            if (randomUserResponse?.Results == null)
            {
                _logger.LogWarning("No results returned from RandomUser API");
                return Array.Empty<RandomUserResult>();
            }

            _logger.LogInformation("Successfully fetched {Count} random users", randomUserResponse.Results.Count);
            return randomUserResponse.Results.ToArray();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error occurred while fetching random users");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error occurred while processing RandomUser API response");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while fetching random users");
            throw;
        }
    }

    public async Task<RandomUserResult> GetRandomUserAsync(string? gender = null, string? nationality = null)
    {
        var users = await GetRandomUsersAsync(1, gender, nationality);
        return users.FirstOrDefault() ?? throw new InvalidOperationException("No user returned from RandomUser API");
    }
}