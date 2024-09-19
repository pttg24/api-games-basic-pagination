using BasicGaming.Api.Models;

namespace BasicGaming.Api.Services;

public interface IGamesService
{
    Task<List<Game>> GetAllGames(string gamesApiUrl);
}
