using McWuT.Data.Models.CrimeGenerator;
using McWuT.Data.Repositories.Base;
using McWuT.Services.CrimeGenerator.External;
using Microsoft.Extensions.Logging;

namespace McWuT.Services.CrimeGenerator;

public interface IGameService
{
    Task<GameSession> CreateNewGameAsync(string userId, DifficultyLevel difficulty = DifficultyLevel.Medium);
    Task<GameSession?> GetGameSessionAsync(Guid gameSessionId, string userId);
    Task<IEnumerable<GameSession>> GetUserGameSessionsAsync(string userId);
    Task<GameSession> UpdateGameSessionAsync(GameSession gameSession);
    Task<bool> MakeAccusationAsync(Guid gameSessionId, string userId, int suspectId);
    Task<GameSession> EndGameAsync(Guid gameSessionId, string userId);
}

public class GameService : IGameService
{
    private readonly IUserEntityRepository<GameSession> _gameRepository;
    private readonly IRandomUserService _randomUserService;
    private readonly ILlmService _llmService;
    private readonly ILogger<GameService> _logger;

    public GameService(
        IUserEntityRepository<GameSession> gameRepository,
        IRandomUserService randomUserService,
        ILlmService llmService,
        ILogger<GameService> logger)
    {
        _gameRepository = gameRepository;
        _randomUserService = randomUserService;
        _llmService = llmService;
        _logger = logger;
    }

    public async Task<GameSession> CreateNewGameAsync(string userId, DifficultyLevel difficulty = DifficultyLevel.Medium)
    {
        try
        {
            _logger.LogInformation("Creating new game session for user {UserId} with difficulty {Difficulty}", 
                userId, difficulty);

            var gameSession = new GameSession
            {
                UserId = userId,
                Title = $"Murder Mystery - {DateTime.UtcNow:yyyy-MM-dd HH:mm}",
                Difficulty = difficulty,
                State = GameSessionState.InProgress,
                StartTime = DateTime.UtcNow
            };

            // Generate initial case description
            gameSession.Description = await _llmService.GenerateCaseDescriptionAsync(
                "Unknown Victim", 
                "Investigation Scene", 
                "Under Investigation"
            );

            var createdGame = await _gameRepository.Create(gameSession);
            
            _logger.LogInformation("Successfully created game session {GameId} for user {UserId}", 
                createdGame.UniqueId, userId);

            return createdGame;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new game session for user {UserId}", userId);
            throw;
        }
    }

    public async Task<GameSession?> GetGameSessionAsync(Guid gameSessionId, string userId)
    {
        try
        {
            var game = await _gameRepository.Get(gameSessionId);
            
            if (game?.UserId != userId)
            {
                _logger.LogWarning("User {UserId} attempted to access game {GameId} they don't own", 
                    userId, gameSessionId);
                return null;
            }

            return game;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving game session {GameId} for user {UserId}", 
                gameSessionId, userId);
            throw;
        }
    }

    public async Task<IEnumerable<GameSession>> GetUserGameSessionsAsync(string userId)
    {
        try
        {
            var result = await _gameRepository.GetAll(
                filter: g => g.UserId == userId,
                orderBy: q => q.OrderByDescending(g => g.CreatedDate)
            );

            return result.Items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving game sessions for user {UserId}", userId);
            throw;
        }
    }

    public async Task<GameSession> UpdateGameSessionAsync(GameSession gameSession)
    {
        try
        {
            gameSession.UpdatedDate = DateTime.UtcNow;
            return await _gameRepository.Update(gameSession);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating game session {GameId}", gameSession.UniqueId);
            throw;
        }
    }

    public async Task<bool> MakeAccusationAsync(Guid gameSessionId, string userId, int suspectId)
    {
        try
        {
            var game = await GetGameSessionAsync(gameSessionId, userId);
            
            if (game == null || game.State != GameSessionState.InProgress)
            {
                return false;
            }

            // For now, we'll implement basic logic - in a real game, this would check against the actual guilty suspect
            var isCorrect = false; // This should be determined by game logic

            game.IsAccusationMade = true;
            game.AccusedSuspectId = suspectId.ToString();
            game.IsCorrectAccusation = isCorrect;
            game.State = isCorrect ? GameSessionState.Solved : GameSessionState.Failed;
            game.EndTime = DateTime.UtcNow;

            await UpdateGameSessionAsync(game);
            
            _logger.LogInformation("User {UserId} made accusation in game {GameId} - Correct: {IsCorrect}", 
                userId, gameSessionId, isCorrect);

            return isCorrect;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing accusation for game {GameId}", gameSessionId);
            throw;
        }
    }

    public async Task<GameSession> EndGameAsync(Guid gameSessionId, string userId)
    {
        try
        {
            var game = await GetGameSessionAsync(gameSessionId, userId);
            
            if (game == null)
            {
                throw new InvalidOperationException("Game session not found");
            }

            if (game.State == GameSessionState.InProgress)
            {
                game.State = GameSessionState.Abandoned;
                game.EndTime = DateTime.UtcNow;
                await UpdateGameSessionAsync(game);
            }

            _logger.LogInformation("Ended game session {GameId} for user {UserId}", gameSessionId, userId);
            
            return game;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ending game session {GameId}", gameSessionId);
            throw;
        }
    }
}