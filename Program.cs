using System;
using System.Collections.Generic;
using System.IO;
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

public class Restaurant : IRestaurant
{
    public string Name { get; set; }
    public ITable[] Tables { get; set; }
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
            var restaurant = new Restaurant { Name = name, Tables = new Table[tableCount] };
            for (int i = 0; i < tableCount; i++)
            {
                restaurant.Tables[i] = new Table();
            }
            restaurants.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    public List<string> FindFreeTables(DateTime dateTime)
    {
        try
        {
            List<string> freeTables = new List<string>();
            foreach (var res in restaurants)
            {
                for (int i = 0; i < res.Tables.Length; i++)
                {
                    if (!res.Tables[i].IsBooked(dateTime))
                    {
                        freeTables.Add($"{res.Name} - Table {i + 1}");
                    }
                }
            }
            return freeTables;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return new List<string>();
        }
    }

    public bool BookTable(string restaurantName, DateTime dateTime, int tableNumber)
    {
        foreach (var res in restaurants)
        {
            if (res.Name == restaurantName)
            {
                if (tableNumber < 0 || tableNumber >= res.Tables.Length)
                {
                    throw new Exception("Invalid table number");
                }

                return res.Tables[tableNumber].Book(dateTime);
            }
        }

        throw new Exception("Restaurant not found");
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
            Console.WriteLine("Error");
        }
    }

    private int CountAvailableTables(IRestaurant restaurant, DateTime dateTime)
    {
        try
        {
            int count = 0;
            foreach (var table in restaurant.Tables)
            {
                if (!table.IsBooked(dateTime))
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return 0;
        }
    }
}