using BasicGaming.Api.Models;
using Newtonsoft.Json;

namespace BasicGaming.Api.Services;

public class GamesService : IGamesService
{
    private readonly HttpClient _httpClient;

    public GamesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Game>> GetAllGames(string gamesApiUrl)
    {
        // Read JSON feed and deserialize into list of Game objects
        var response = await _httpClient.GetAsync(gamesApiUrl);
        var json = await response.Content.ReadAsStringAsync();
        var games = JsonConvert.DeserializeObject<List<Game>>(json) ?? new List<Game>();

        return games;
    }
}
