using BasicGaming.Api.Models;
using BasicGaming.Api.Utils;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BasicGaming.Api.Tests.Utils;

public class ListExtensionTests
{
    private List<Game> GetMockGamesList()
    {
        return new List<Game>
        {
            new Game { appid = 1, name = "Game One", release_date = new DateTime(2023, 1, 15) },
            new Game { appid = 2, name = "Game Two", release_date = new DateTime(2023, 1, 20) },
            new Game { appid = 3, name = "Game Three", release_date = new DateTime(2022, 12, 15) },
            new Game { appid = 4, name = "Game Four", release_date = new DateTime(2022, 10, 20) },
            new Game { appid = 5, name = "Game Five", release_date = new DateTime(2021, 5, 10) }
        };
    }

    [Fact]
    public void PaginateWithOffsetAndLimit_ShouldReturnCorrectSubsetOfGames()
    {
        // Arrange
        var mockGames = GetMockGamesList();
        int offset = 1, limit = 2;

        // Act
        var result = mockGames.PaginateWithOffsetAndLimit(offset, limit);

        // Assert
        result.Should().NotBeNull();
        result.items.Should().HaveCount(2);
        result.totalItems.Should().Be(5); // Total games in original list
        result.items.First().Id.Should().Be(2); // Skipped first game, so the first result should have Id = 2
        result.items.Last().Id.Should().Be(3);  // The second item in the result should have Id = 3
    }

    [Fact]
    public void PaginateWithOffsetAndLimit_ShouldReturnEmptyList_WhenOffsetExceedsTotalGames()
    {
        // Arrange
        var mockGames = GetMockGamesList();
        int offset = 10, limit = 2; // Offset beyond the list size

        // Act
        var result = mockGames.PaginateWithOffsetAndLimit(offset, limit);

        // Assert
        result.Should().NotBeNull();
        result.items.Should().BeEmpty(); // No items should be returned
        result.totalItems.Should().Be(5); // Total items remain the same (5)
    }

    [Fact]
    public void PaginateWithPageAndPageSize_ShouldReturnCorrectSubsetOfGames()
    {
        // Arrange
        var mockGames = GetMockGamesList();
        int page = 2, pageSize = 2;

        // Act
        var result = mockGames.PaginateWithPageAndPageSize(page, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.items.Should().HaveCount(2);
        result.totalItems.Should().Be(5); // Total games in original list
        result.items.First().Id.Should().Be(3); // The first item on page 2 should have Id = 3
        result.items.Last().Id.Should().Be(4);  // The second item on page 2 should have Id = 4
    }

    [Fact]
    public void PaginateWithPageAndPageSize_ShouldReturnEmptyList_WhenPageExceedsTotalGames()
    {
        // Arrange
        var mockGames = GetMockGamesList();
        int page = 5, pageSize = 2; // Page exceeds total number of games

        // Act
        var result = mockGames.PaginateWithPageAndPageSize(page, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.items.Should().BeEmpty(); // No items should be returned
        result.totalItems.Should().Be(5); // Total items remain the same (5)
    }

    [Fact]
    public void PaginateWithOffsetAndLimit_ShouldHandleZeroLimit()
    {
        // Arrange
        var mockGames = GetMockGamesList();
        int offset = 0, limit = 0;

        // Act
        var result = mockGames.PaginateWithOffsetAndLimit(offset, limit);

        // Assert
        result.Should().NotBeNull();
        result.items.Should().BeEmpty(); // No items should be returned
        result.totalItems.Should().Be(5); // Total items remain the same (5)
    }

    [Fact]
    public void PaginateWithPageAndPageSize_ShouldHandleZeroPageSize()
    {
        // Arrange
        var mockGames = GetMockGamesList();
        int page = 1, pageSize = 0;

        // Act
        var result = mockGames.PaginateWithPageAndPageSize(page, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.items.Should().BeEmpty(); // No items should be returned
        result.totalItems.Should().Be(5); // Total items remain the same (5)
    }
}
