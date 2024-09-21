using BasicGaming.Api.Models;

namespace BasicGaming.Api.Utils;

public static class ListExtension
{
    public static GamesResult PaginateWithOffsetAndLimit(this List<Game> games, int offset, int limit)
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

    public static GamesResult PaginateWithPageAndPageSize(this List<Game> games, int page, int pageSize)
    {
        // Apply pagination
        var totalItems = games.Count;
        games = games.Skip((page - 1) * pageSize).Take(pageSize).ToList();
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
