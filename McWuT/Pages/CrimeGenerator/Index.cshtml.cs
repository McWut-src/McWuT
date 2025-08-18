using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using McWuT.Services.CrimeGenerator;
using McWuT.Data.Models.CrimeGenerator;
using Microsoft.AspNetCore.Identity;

namespace McWuT.Web.Pages.CrimeGenerator;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IGameService _gameService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        IGameService gameService, 
        UserManager<IdentityUser> userManager,
        ILogger<IndexModel> logger)
    {
        _gameService = gameService;
        _userManager = userManager;
        _logger = logger;
    }

    public IEnumerable<GameSession>? UserGameSessions { get; set; }

    public async Task OnGetAsync()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                try
                {
                    UserGameSessions = await _gameService.GetUserGameSessionsAsync(user.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading game sessions for user {UserId}", user.Id);
                }
            }
        }
    }

    public async Task<IActionResult> OnPostCreateGameAsync(DifficultyLevel difficulty = DifficultyLevel.Medium)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var newGame = await _gameService.CreateNewGameAsync(user.Id, difficulty);
            
            _logger.LogInformation("Created new game {GameId} for user {UserId}", 
                newGame.UniqueId, user.Id);

            return RedirectToPage("./Game", new { id = newGame.UniqueId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new game");
            TempData["ErrorMessage"] = "Failed to create new game. Please try again.";
            return Page();
        }
    }
}