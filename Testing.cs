using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class TableTests
{
    [Fact]
    public void Book_ValidDate_ShouldReturnTrue()
    {
        // Arrange
        var table = new Table();
        var dateTime = new DateTime(2023, 12, 25);

        // Act
        var result = table.Book(dateTime);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Book_AlreadyBookedDate_ShouldReturnFalse()
    {
        // Arrange
        var table = new Table();
        var dateTime = new DateTime(2023, 12, 25);
        table.Book(dateTime);

        // Act
        var result = table.Book(dateTime);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsBooked_BookedDate_ShouldReturnTrue()
    {
        // Arrange
        var table = new Table();
        var dateTime = new DateTime(2023, 12, 25);
        table.Book(dateTime);

        // Act
        var result = table.IsBooked(dateTime);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsBooked_NotBookedDate_ShouldReturnFalse()
    {
        // Arrange
        var table = new Table();
        var dateTime = new DateTime(2023, 12, 25);

        // Act
        var result = table.IsBooked(dateTime);

        // Assert
        Assert.False(result);
    }
}

public class ReservationManagerTests
{
    [Fact]
    public void AddRestaurant_ValidInput_ShouldAddRestaurant()
    {
        // Arrange
        var manager = new ReservationManager();

        // Act
        manager.AddRestaurant("C", 8);

        // Assert
        Assert.Equal(1, manager.Restaurants.Count);
        Assert.Equal("C", manager.Restaurants.First().Name);
    }

    [Fact]
    public void BookTable_ValidInput_ShouldReturnTrue()
    {
        // Arrange
        var manager = new ReservationManager();
        manager.AddRestaurant("D", 5);

        // Act
        var result = manager.BookTable("D", new DateTime(2023, 12, 25), 3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void BookTable_InvalidRestaurant_ShouldReturnFalse()
    {
        // Arrange
        var manager = new ReservationManager();

        // Act
        var result = manager.BookTable("NonExistent", new DateTime(2023, 12, 25), 3);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SortRestaurantsByAvailability_ValidInput_ShouldSortByAvailability()
    {
        // Arrange
        var manager = new ReservationManager();
        manager.AddRestaurant("E", 3);
        manager.AddRestaurant("F", 5);
        manager.AddRestaurant("G", 2);

        // Act
        manager.SortRestaurantsByAvailability(new DateTime(2023, 12, 25));

        // Assert
        Assert.Equal("F", manager.Restaurants[0].Name);
        Assert.Equal("E", manager.Restaurants[1].Name);
        Assert.Equal("G", manager.Restaurants[2].Name);
    }

    [Fact]
    public void FindFreeTables_ValidDate_ShouldReturnListOfFreeTables()
    {
        // Arrange
        var manager = new ReservationManager();
        manager.AddRestaurant("H", 3);
        manager.AddRestaurant("I", 2);

        // Act
        var result = manager.FindFreeTables(new DateTime(2023, 12, 25));

        // Assert
        Assert.Equal(5, result.Count);
        Assert.Contains("H - Table 1", result);
        Assert.Contains("I - Table 2", result);
    }
}
