namespace BasicGaming.Api.Models;

public class Game
{
    public int appid { get; set; }
    public string? name { get; set; }
    public string? short_description { get; set; }
    public string? publisher { get; set; }
    public string? genre { get; set; }
    public List<string>? categories { get; set; }
    public Dictionary<string, bool>? platforms { get; set; }
    public DateTime release_date { get; set; }
    public string? required_age { get; set; }
}
