using BasicGaming.Api.Constants;
using BasicGaming.Api.Models;
using BasicGaming.Api.Services;
using BasicGaming.Api.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicGaming.Api.Controllers;

[ApiController]
[Produces("application/json")]
public class GamesController : ControllerBase
{
    private readonly IValidator<PaginationAInputDTO> _paginationAInputValidator;
    private readonly IValidator<PaginationBInputDTO> _paginationBInputValidator;
    private readonly IGamesService _gamesService;

    private const string FeedUrl = "https://pttg24.github.io/api-games-basic-pagination/steam_games_feed.json";

    public GamesController(
        IValidator<PaginationAInputDTO> paginationAInputValidator,
        IValidator<PaginationBInputDTO> paginationBInputValidator,
        IGamesService gamesService
        )
    {
        _paginationAInputValidator = paginationAInputValidator;
        _paginationBInputValidator = paginationBInputValidator;
        _gamesService = gamesService;
    }

    [HttpGet(ApiRoutes.Implementation1)]
    public async Task<IActionResult> GetWithOffsetAndLimit([FromQuery] int offset = 0, [FromQuery] int limit = 2)
    {
        try
        {
            // Validate User-Agent header
            if (!Request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                return BadRequest("User-Agent is not provided.");
            }

            // Validate input
            var paginationInput = new PaginationAInputDTO
            {
                Offset = offset,
                Limit = limit
            };

            var validationResult = _paginationAInputValidator.Validate(paginationInput);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            // Get Games data
            var games = await _gamesService.GetAllGames(FeedUrl);

            // Order by releaseDate in descending way
            games = games.OrderByDescending(g => g.release_date).ToList();

            var result = games.PaginateWithOffsetAndLimit(offset, limit);

            return Ok(result);
        }
        catch (Exception)
        {
            return Problem("Something went wrong :(");
        }
    }

    [HttpGet(ApiRoutes.Implementation2)]
    public async Task<IActionResult> GetWithPageAndPageSizeAndOrderBy([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string orderBy = "desc")
    {
        try
        {
            // Validate User-Agent header
            if (!Request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                return BadRequest("User-Agent is not provided.");
            }

            // Validate input
            var paginationInput = new PaginationBInputDTO
            {
                Page = page,
                PageSize = pageSize,
                OrderBy = orderBy
            };

            var validationResult = _paginationBInputValidator.Validate(paginationInput);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            // Get Games data
            var games = await _gamesService.GetAllGames(FeedUrl);

            // Order by releaseDate + check OrderBy
            bool orderByDesc = orderBy.EndsWith("desc", StringComparison.OrdinalIgnoreCase) ? true : false;
            if (orderByDesc)
                games = games.OrderByDescending(g => g.release_date).ToList();
            else
                games = games.OrderBy(g => g.release_date).ToList();

            var result = games.PaginateWithPageAndPageSize(page, pageSize);

            return Ok(result);
        }
        catch (Exception)
        {
            return Problem("Something went wrong :(");
        }
    }
}
