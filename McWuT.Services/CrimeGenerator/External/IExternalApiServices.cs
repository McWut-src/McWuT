using McWuT.Common.CrimeGenerator.ExternalApis;

namespace McWuT.Services.CrimeGenerator.External;

public interface IRandomUserService
{
    Task<RandomUserResult[]> GetRandomUsersAsync(int count = 1, string? gender = null, string? nationality = null);
    Task<RandomUserResult> GetRandomUserAsync(string? gender = null, string? nationality = null);
}

public interface ILlmService
{
    Task<string> GenerateTextAsync(string prompt, int maxTokens = 1000, double temperature = 0.7);
    Task<string> GenerateBackstoryAsync(string characterName, string occupation, int age, string personality);
    Task<string> GenerateMotiveAsync(string suspectName, string victimName, string relationship);
    Task<string> GenerateAlibiAsync(string suspectName, DateTime timeOfCrime);
    Task<string> GenerateDialogueAsync(string characterName, string context, string question);
    Task<string> GenerateCaseDescriptionAsync(string victimName, string location, string causeOfDeath);
    Task<string> GenerateClueDescriptionAsync(string clueType, string location, string relevance);
}