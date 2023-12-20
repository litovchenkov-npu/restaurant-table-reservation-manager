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
        try
        {
            if (bookedDates.Contains(dateTime))
            {
                return false;
            }
            bookedDates.Add(dateTime);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error booking table: {ex.Message}");
            return false;
        }
    }

    public bool IsBooked(DateTime dateTime)
    {
        try
        {
            return bookedDates.Contains(dateTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking table availability: {ex.Message}");
            return false;
        }
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
        try
        {
            var restaurant = new Restaurant { Name = name, Tables = Enumerable.Range(0, tableCount).Select(_ => new Table()).ToArray() };
            restaurants.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding restaurant: {ex.Message}");
        }
    }

    public List<string> FindFreeTables(DateTime dateTime)
    {
        try
        {
            return restaurants
                .SelectMany(res => res.Tables.Select((table, index) => new { Restaurant = res, TableIndex = index, IsBooked = table.IsBooked(dateTime) }))
                .Where(t => !t.IsBooked)
                .Select(t => $"{t.Restaurant.Name} - Table {t.TableIndex + 1}")
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finding free tables: {ex.Message}");
            return new List<string>();
        }
    }

    public bool BookTable(string restaurantName, DateTime dateTime, int tableNumber)
    {
        try
        {
            var restaurant = restaurants.FirstOrDefault(res => res.Name == restaurantName);

            if (restaurant == null || tableNumber < 0 || tableNumber >= restaurant.Tables.Length)
            {
                return false;
            }

            return restaurant.Tables[tableNumber].Book(dateTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error booking table: {ex.Message}");
            return false;
        }
    }

    public void SortRestaurantsByAvailability(DateTime dateTime)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error sorting restaurants: {ex.Message}");
        }
    }

    private int CountAvailableTables(IRestaurant restaurant, DateTime dateTime)
    {
        try
        {
            return restaurant.Tables.Count(table => !table.IsBooked(dateTime));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error counting available tables: {ex.Message}");
            return 0;
        }
    }
}