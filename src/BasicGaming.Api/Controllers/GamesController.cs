using BasicGaming.Api.Models;
using BasicGaming.Api.Services;
using BasicGaming.Api.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicGaming.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class GamesController : ControllerBase
{
    private readonly IValidator<PaginationInputDTO> _paginationInputValidator;
    private readonly IGamesService _gamesService;

    private const string FeedUrl = "https://pttg24.github.io/api-games-basic-pagination/steam_games_feed.json";

    public GamesController(IValidator<PaginationInputDTO> paginationInputValidator, IGamesService gamesService)
    {
        _paginationInputValidator = paginationInputValidator;
        _gamesService = gamesService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int offset = 0, [FromQuery] int limit = 2)
    {
        try
        {
            // Validate User-Agent header
            if (!Request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                return BadRequest("User-Agent is not provided.");
            }

            // Validate input
            var paginationInput = new PaginationInputDTO
            {
                Offset = offset,
                Limit = limit
            };

            var validationResult = _paginationInputValidator.Validate(paginationInput);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            // Get Games data
            var games = await _gamesService.GetAllGames(FeedUrl);

            // Order by releaseDate in descending way
            games = games.OrderByDescending(g => g.release_date).ToList();

            var result = games.Paginate(offset, limit);

            return Ok(result);
        }
        catch (Exception)
        {
            return Problem("Something went wrong :(");
        }
    }
}
