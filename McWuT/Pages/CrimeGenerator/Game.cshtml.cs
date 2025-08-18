using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using McWuT.Services.CrimeGenerator;
using McWuT.Data.Models.CrimeGenerator;
using Microsoft.AspNetCore.Identity;

namespace McWuT.Web.Pages.CrimeGenerator;

[Authorize]
public class GameModel : PageModel
{
    private readonly IGameService _gameService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<GameModel> _logger;

    public GameModel(
        IGameService gameService,
        UserManager<IdentityUser> userManager,
        ILogger<GameModel> logger)
    {
        _gameService = gameService;
        _userManager = userManager;
        _logger = logger;
    }

    public GameSession? GameSession { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        try
        {
            GameSession = await _gameService.GetGameSessionAsync(id.Value, user.Id);
            
            if (GameSession == null)
            {
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading game session {GameId} for user {UserId}", 
                id, user.Id);
            return NotFound();
        }
    }
}