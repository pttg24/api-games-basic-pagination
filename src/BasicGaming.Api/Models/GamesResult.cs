namespace BasicGaming.Api.Models;
public class GamesResult
{
    public IEnumerable<GameDTO>? items { get; set; }
    public int totalItems { get; set; }
}
