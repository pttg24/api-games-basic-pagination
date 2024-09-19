namespace BasicGaming.Api.Models;
public class GameDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Publisher { get; set; }
    public string? Genre { get; set; }
    public List<string>? Categories { get; set; }
    public Dictionary<string, bool>? Platforms { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int RequiredAge { get; set; }

    public GameDTO(Game game)
    {
        Id = game.appid;
        Name = game.name;
        ShortDescription = game.short_description;
        Publisher = game.publisher;
        Genre = game.genre;
        Categories = game.categories;
        Platforms = game.platforms;
        ReleaseDate = game.release_date;
        if (int.TryParse(game.required_age, out int requiredAge))
        {
            RequiredAge = requiredAge;
        }
        else if(game.required_age != null && int.TryParse(game.required_age.Replace("+", ""), out int requiredAgePlus))
        {
            RequiredAge = requiredAgePlus;
        }
        else
        {
            RequiredAge = 0;
        }
    }
}
