using BasicGaming.Api.Models;

namespace BasicGaming.Api.Utils;

public static class ListExtension
{
    public static GamesResult Paginate(this List<Game> games, int offset, int limit)
    {
        // Apply pagination
        var totalItems = games.Count;
        games = games.Skip(offset).Take(limit).ToList();
        var gameDTOs = games.Select(game => new GameDTO(game));

        // Return paged result in same format as example provided
        var result = new GamesResult
        {
            items = gameDTOs,
            totalItems = totalItems
        };

        return result;
    }
}
