using System;
using System.Collections.Generic;
using System.Linq;

public interface ITable
{
    bool Book(DateTime dateTime);
    bool IsBooked(DateTime dateTime);
}

public interface IRestaurant
{
    string Name { get; }
    ITable[] Tables { get; }
}

public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManager manager = new ReservationManager();
        manager.AddRestaurant("A", 10);
        manager.AddRestaurant("B", 5);

        Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

public class Table : ITable
{
    private List<DateTime> bookedDates;

    public Table()
    {
        bookedDates = new List<DateTime>();
    }

    public bool Book(DateTime dateTime)
    {
        if (bookedDates.Contains(dateTime))
        {
            return false;
        }
        bookedDates.Add(dateTime);
        return true;
    }

    public bool IsBooked(DateTime dateTime)
    {
        return bookedDates.Contains(dateTime);
    }
}

public class Restaurant : IRestaurant
{
    public string Name { get; set; }
    public ITable[] Tables { get; set; }
}

public class ReservationManager
{
    private List<IRestaurant> restaurants;

    public ReservationManager()
    {
        restaurants = new List<IRestaurant>();
    }

    public void AddRestaurant(string name, int tableCount)
    {
        var restaurant = new Restaurant { Name = name, Tables = Enumerable.Range(0, tableCount).Select(_ => new Table()).ToArray() };
        restaurants.Add(restaurant);
    }

    public List<string> FindFreeTables(DateTime dateTime)
    {
        return restaurants
            .SelectMany(res => res.Tables.Select((table, index) => new { Restaurant = res, TableIndex = index, IsBooked = table.IsBooked(dateTime) }))
            .Where(t => !t.IsBooked)
            .Select(t => $"{t.Restaurant.Name} - Table {t.TableIndex + 1}")
            .ToList();
    }

    public bool BookTable(string restaurantName, DateTime dateTime, int tableNumber)
    {
        var restaurant = restaurants.FirstOrDefault(res => res.Name == restaurantName);

        if (restaurant == null || tableNumber < 0 || tableNumber >= restaurant.Tables.Length)
        {
            return false;
        }

        return restaurant.Tables[tableNumber].Book(dateTime);
    }

    public void SortRestaurantsByAvailability(DateTime dateTime)
    {
        bool swapped;
        do
        {
            swapped = false;
            for (int i = 0; i < restaurants.Count - 1; i++)
            {
                int currentTables = CountAvailableTables(restaurants[i], dateTime);
                int nextTables = CountAvailableTables(restaurants[i + 1], dateTime);

                if (currentTables < nextTables)
                {
                    var temp = restaurants[i];
                    restaurants[i] = restaurants[i + 1];
                    restaurants[i + 1] = temp;
                    swapped = true;
                }
            }
        } while (swapped);
    }

    private int CountAvailableTables(IRestaurant restaurant, DateTime dateTime)
    {
        return restaurant.Tables.Count(table => !table.IsBooked(dateTime));
    }
}