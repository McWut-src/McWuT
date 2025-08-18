using McWuT.Common.CrimeGenerator.ExternalApis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace McWuT.Services.CrimeGenerator.External;

public class LlmService : ILlmService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LlmService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string? _apiKey;
    private readonly string _baseUrl;

    public LlmService(HttpClient httpClient, ILogger<LlmService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        _apiKey = _configuration["OpenAI:ApiKey"];
        _baseUrl = _configuration["OpenAI:BaseUrl"] ?? "https://api.openai.com/v1/chat/completions";

        if (!string.IsNullOrEmpty(_apiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }
    }

    public async Task<string> GenerateTextAsync(string prompt, int maxTokens = 1000, double temperature = 0.7)
    {
        try
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                _logger.LogWarning("OpenAI API key not configured, returning fallback response");
                return GenerateFallbackResponse(prompt);
            }

            var request = new LlmRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = new List<LlmMessage>
                {
                    new() { Role = "user", Content = prompt }
                },
                MaxTokens = maxTokens,
                Temperature = temperature
            };

            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            _logger.LogInformation("Sending request to OpenAI API");
            var response = await _httpClient.PostAsync(_baseUrl, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("OpenAI API request failed: {StatusCode} - {Content}", 
                    response.StatusCode, errorContent);
                return GenerateFallbackResponse(prompt);
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var llmResponse = JsonSerializer.Deserialize<LlmResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            var result = llmResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim();
            
            if (string.IsNullOrEmpty(result))
            {
                _logger.LogWarning("Empty response from OpenAI API");
                return GenerateFallbackResponse(prompt);
            }

            _logger.LogInformation("Successfully generated text with {TokenCount} tokens", 
                llmResponse?.Usage?.TotalTokens ?? 0);
            
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error occurred while calling OpenAI API");
            return GenerateFallbackResponse(prompt);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON error occurred while processing OpenAI API response");
            return GenerateFallbackResponse(prompt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while calling OpenAI API");
            return GenerateFallbackResponse(prompt);
        }
    }

    public async Task<string> GenerateBackstoryAsync(string characterName, string occupation, int age, string personality)
    {
        var prompt = $"Generate a detailed backstory for a murder mystery character named {characterName}, " +
                    $"who is a {age}-year-old {occupation} with a {personality} personality. " +
                    "Include relevant details about their past, relationships, and current life situation. " +
                    "Keep it under 200 words and make it compelling for a murder mystery game.";

        return await GenerateTextAsync(prompt, 300, 0.8);
    }

    public async Task<string> GenerateMotiveAsync(string suspectName, string victimName, string relationship)
    {
        var prompt = $"Generate a plausible motive for {suspectName} to murder {victimName}. " +
                    $"Their relationship is: {relationship}. " +
                    "The motive should be believable and compelling for a murder mystery game. " +
                    "Keep it under 150 words.";

        return await GenerateTextAsync(prompt, 200, 0.7);
    }

    public async Task<string> GenerateAlibiAsync(string suspectName, DateTime timeOfCrime)
    {
        var prompt = $"Generate a detailed alibi for {suspectName} for the time period around {timeOfCrime:yyyy-MM-dd HH:mm}. " +
                    "The alibi should be somewhat believable but may have some inconsistencies or gaps " +
                    "that investigators could discover. Keep it under 100 words.";

        return await GenerateTextAsync(prompt, 150, 0.6);
    }

    public async Task<string> GenerateDialogueAsync(string characterName, string context, string question)
    {
        var prompt = $"Generate a realistic response from {characterName} in a murder mystery investigation. " +
                    $"Context: {context}. " +
                    $"Question asked: {question}. " +
                    "The response should be in character, reveal some information but also potentially " +
                    "hide something or be evasive. Keep it conversational and under 100 words.";

        return await GenerateTextAsync(prompt, 150, 0.8);
    }

    public async Task<string> GenerateCaseDescriptionAsync(string victimName, string location, string causeOfDeath)
    {
        var prompt = $"Generate a compelling case description for a murder mystery game. " +
                    $"Victim: {victimName}, Location: {location}, Cause of Death: {causeOfDeath}. " +
                    "Include atmospheric details and initial investigation findings. " +
                    "Make it engaging and mysterious. Keep it under 250 words.";

        return await GenerateTextAsync(prompt, 350, 0.7);
    }

    public async Task<string> GenerateClueDescriptionAsync(string clueType, string location, string relevance)
    {
        var prompt = $"Generate a description for a {clueType} clue found at {location}. " +
                    $"The clue is {relevance} to solving the murder mystery. " +
                    "Describe what investigators would see and how it might be relevant. " +
                    "Keep it under 100 words and make it intriguing.";

        return await GenerateTextAsync(prompt, 150, 0.6);
    }

    private string GenerateFallbackResponse(string prompt)
    {
        // Provide basic fallback responses when LLM is not available
        if (prompt.Contains("backstory", StringComparison.OrdinalIgnoreCase))
        {
            return "A mysterious individual with a complex past and hidden secrets that may be relevant to the investigation.";
        }
        
        if (prompt.Contains("motive", StringComparison.OrdinalIgnoreCase))
        {
            return "A compelling reason rooted in personal conflict, financial gain, or revenge.";
        }
        
        if (prompt.Contains("alibi", StringComparison.OrdinalIgnoreCase))
        {
            return "Claims to have been elsewhere during the time of the crime, but the details are somewhat vague.";
        }

        if (prompt.Contains("dialogue", StringComparison.OrdinalIgnoreCase))
        {
            return "I'm not sure I can help you with that. You'd have to ask someone else.";
        }

        return "This information requires further investigation to uncover the full details.";
    }
}