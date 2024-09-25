using BasicGaming.Api.Models;
using BasicGaming.Api.Services;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BasicGaming.Api.Tests.Services;

public class GamesServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly GamesService _gamesService;

    public GamesServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _gamesService = new GamesService(_httpClient);
    }

    [Fact]
    public async Task GetAllGames_ShouldReturnListOfGames_WhenApiReturnsValidJson()
    {
        // Arrange
        var mockGames = new List<Game>
        {
            new()
            {
                appid = 1,
                name = "Game One",
                short_description = "An action game",
                publisher = "Epic Games",
                genre = "Action",
                release_date = new DateTime(2023, 1, 15)
            },
            new()
            {
                appid = 2,
                name = "Game Two",
                short_description = "A RPG game",
                publisher = "Square Enix",
                genre = "RPG",
                release_date = new DateTime(2022, 7, 20)
            }
        };

        var jsonResponse = JsonConvert.SerializeObject(mockGames);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var result = await _gamesService.GetAllGames("https://fakeapi.com/games");

        // Assert
        result.Should().BeOfType<List<Game>>();
        result.Should().HaveCount(2);
        result[0].name.Should().Be("Game One");
        result[1].name.Should().Be("Game Two");
    }

    [Fact]
    public async Task GetAllGames_ShouldReturnEmptyList_WhenApiReturnsEmptyJson()
    {
        // Arrange
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[]")
            });

        // Act
        var result = await _gamesService.GetAllGames("https://fakeapi.com/games");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllGames_ShouldHandleNullResponseContent()
    {
        // Arrange
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            });

        // Act
        var result = await _gamesService.GetAllGames("https://fakeapi.com/games");

        // Assert
        result.Should().BeEmpty();  // Should return an empty list if content is null or empty
    }
}
