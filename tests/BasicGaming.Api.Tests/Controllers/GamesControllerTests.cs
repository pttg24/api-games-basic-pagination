using Xunit;
using Moq;
using FluentAssertions;
using BasicGaming.Api.Controllers;
using BasicGaming.Api.Models;
using BasicGaming.Api.Services;
using BasicGaming.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace BasicGaming.Api.Tests.Controllers;

public class GamesControllerTests
{
    private readonly Mock<IValidator<PaginationAInputDTO>> _paginationAValidatorMock;
    private readonly Mock<IValidator<PaginationBInputDTO>> _paginationBValidatorMock;
    private readonly Mock<IGamesService> _gamesServiceMock;
    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _paginationAValidatorMock = new Mock<IValidator<PaginationAInputDTO>>();
        _paginationBValidatorMock = new Mock<IValidator<PaginationBInputDTO>>();
        _gamesServiceMock = new Mock<IGamesService>();

        _controller = new GamesController(
            _paginationAValidatorMock.Object,
            _paginationBValidatorMock.Object,
            _gamesServiceMock.Object
        );

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["User-Agent"] = "Test-Agent";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetWithOffsetAndLimit_ValidInput_ReturnsOkResult()
    {
        // Arrange
        var paginationInput = new PaginationAInputDTO { Offset = 0, Limit = 2 };
        _paginationAValidatorMock
            .Setup(v => v.Validate(It.IsAny<PaginationAInputDTO>()))
            .Returns(new ValidationResult());

        var mockGames = GetMockGamesList();
        _gamesServiceMock.Setup(service => service.GetAllGames(It.IsAny<string>()))
            .ReturnsAsync(mockGames);

        // Act
        var result = await _controller.GetWithOffsetAndLimit(0, 2);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<GamesResult>();

        var returnedGames = result.As<OkObjectResult>().Value as GamesResult;
        returnedGames.items.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetWithOffsetAndLimit_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var validationFailure = new List<ValidationFailure> { 
            new ValidationFailure("Limit", "Limit must be greater than 0") };
        _paginationAValidatorMock
            .Setup(v => v.Validate(It.IsAny<PaginationAInputDTO>()))
            .Returns(new ValidationResult(validationFailure));

        // Act
        var result = await _controller.GetWithOffsetAndLimit(0, 0);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();

        var errors = result.As<BadRequestObjectResult>().Value as IEnumerable<string>;
        errors.Should().NotBeNull();
        errors.First().Should().Be("Limit must be greater than 0");
    }

    [Fact]
    public async Task GetWithOffsetAndLimit_MissingUserAgent_ReturnsBadRequest()
    {
        // Arrange
        _controller.ControllerContext.HttpContext.Request.Headers.Remove("User-Agent");

        // Act
        var result = await _controller.GetWithOffsetAndLimit(0, 2);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("User-Agent is not provided.");
    }

    [Fact]
    public async Task GetWithPageAndPageSizeAndOrderBy_ValidInput_ReturnsOkResult()
    {
        // Arrange
        var paginationInput = new PaginationBInputDTO { Page = 1, PageSize = 2, OrderBy = "desc" };
        _paginationBValidatorMock
            .Setup(v => v.Validate(It.IsAny<PaginationBInputDTO>()))
            .Returns(new ValidationResult());

        var mockGames = GetMockGamesList();
        _gamesServiceMock.Setup(service => service.GetAllGames(It.IsAny<string>()))
            .ReturnsAsync(mockGames);

        // Act
        var result = await _controller.GetWithPageAndPageSizeAndOrderBy(1, 2, "desc");

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<GamesResult>();

        var returnedGames = result.As<OkObjectResult>().Value as GamesResult;
        returnedGames.items.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetWithPageAndPageSizeAndOrderBy_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var validationFailure = new List<ValidationFailure> { new ValidationFailure("PageSize", "PageSize must be greater than 0") };
        _paginationBValidatorMock
            .Setup(v => v.Validate(It.IsAny<PaginationBInputDTO>()))
            .Returns(new ValidationResult(validationFailure));

        // Act
        var result = await _controller.GetWithPageAndPageSizeAndOrderBy(1, 0, "asc");

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();

        var errors = result.As<BadRequestObjectResult>().Value as IEnumerable<string>;
        errors.Should().NotBeNull();
        errors.First().Should().Be("PageSize must be greater than 0");
    }

    private List<Game> GetMockGamesList()
    {
        return new List<Game>
        {
            new() 
            {
                appid = 1,
                name = "Game One",
                short_description = "A thrilling action game.",
                publisher = "Epic Games",
                genre = "Action",
                categories = new List<string> { "Multiplayer", "Shooter" },
                platforms = new Dictionary<string, bool>
                {
                    { "Windows", true },
                    { "Mac", false },
                    { "Linux", true }
                },
                release_date = new DateTime(2023, 1, 15),
                required_age = "18"
            },
            new()
            {
                appid = 2,
                name = "Game Two",
                short_description = "A magical RPG adventure.",
                publisher = "Square Enix",
                genre = "RPG",
                categories = new List<string> { "Singleplayer", "Fantasy" },
                platforms = new Dictionary<string, bool>
                {
                    { "Windows", true },
                    { "Mac", true },
                    { "Linux", false }
                },
                release_date = new DateTime(2022, 7, 20),
                required_age = "12+"
            }
        };
    }

}
