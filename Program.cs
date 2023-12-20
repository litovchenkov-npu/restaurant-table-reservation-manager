
using System;
using System.Collections.Generic;

// Main Application Class
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

// Reservation Manager Class
public class ReservationManager
{
    // res
    public List<Restaurant> reservation;

    public ReservationManager()
    {
        reservation = new List<Restaurant>();
    }

    // Add Restaurant Method
    public void AddRestaurant(string name, int table)
    {
        try
        {
            Restaurant restaurant = new Restaurant();
            restaurant.name = name;
            restaurant.tables = new Table[table];
            for (int i = 0; i < table; i++)
            {
                restaurant.tables[i] = new Table();
            }
            reservation.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // Load Restaurants From
    // File
    private void LoadRestaurantsFromFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurant(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    //Find All Free Tables
    public List<string> FindFreeTables(DateTime dateTime)
    {
        try
        { 
            List<string> freeTables = new List<string>();
            foreach (var res in reservation)
            {
                for (int i = 0; i < res.tables.Length; i++)
                {
                    if (!res.tables[i].IsBooked(dateTime))
                    {
                        freeTables.Add($"{res.name} - Table {i + 1}");
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
        foreach (var res in reservation)
        {
            if (res.name == restaurantName)
            {
                if (tableNumber < 0 || tableNumber >= res.tables.Length)
                {
                    throw new Exception(null); //Invalid table number
                }

                return res.tables[tableNumber].Book(dateTime);
            }
        }

        throw new Exception(null); //Restaurant not found
    }

    public void SortRestaurantsByAvailability(DateTime dateTime)
    {
        try
        { 
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < reservation.Count - 1; i++)
                {
                    int currentTables = CountAvailableTables(reservation[i], dateTime); // available tables current
                    int nextTables = CountAvailableTables(reservation[i + 1], dateTime); // available tables next

                    if (currentTables < nextTables)
                    {
                        // Swap restaurants
                        var temp = reservation[i];
                        reservation[i] = reservation[i + 1];
                        reservation[i + 1] = temp;
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

    // count available tables in a restaurant
    public int CountAvailableTables(Restaurant restaurant, DateTime dateTime)
    {
        try
        {
            int count = 0;
            foreach (var table in restaurant.tables)
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

// Restaurant Class
public class Restaurant
{
    public string name; //name
    public Table[] tables; // tables
}

// Table Class
public class Table
{
    private List<DateTime> bookedDates; //booked dates

    public Table()
    {
        bookedDates = new List<DateTime>();
    }

    // book
    public bool Book(DateTime dateTime)
    {
        try
        { 
            if (bookedDates.Contains(dateTime))
            {
                return false;
            }
            //add to bd
            bookedDates.Add(dateTime);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return false;
        }
    }

    // is booked
    public bool IsBooked(DateTime dateTime)
    {
        return bookedDates.Contains(dateTime);
    }
}