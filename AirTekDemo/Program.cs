using AirTekDemo.Model;
using Newtonsoft.Json.Linq;
using System.IO;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Coding assessment for Air-Tek");

        int orderCapacity = 20;
        //USER STORY #1
        var flights = LoadFlights(orderCapacity);
        DisplayFlightsDetails(flights);


        //USER STORY #2
        var orders = LoadOrdersFromJson("coding-assigment-orders.json");
        var flightOrderDetails = GenerateFlightOrders(flights, orders);
        DisplayFlightOrders(flightOrderDetails);
        Console.ReadLine();


        
    }
    // Method to display flight schedule
    static void DisplayFlightsDetails(List<Flight> flights)
    {
        Console.WriteLine("Flight Schedule:");
        foreach (var flight in flights)
        {
            Console.WriteLine($"Flight: {flight.FlightId}, Departure: {flight.DepartureDetail} ({flight.DepartureCode}), Arrival: {flight.ArrivalDetail} ({flight.ArrivalCode}), day: {flight.Day}");
        }
        Console.WriteLine();
    }


    //Get Hard code flight information
    static List<Flight> LoadFlights(int orderCapacity)
    {
        var flights = new List<Flight>
            {
                new Flight(){FlightId=1, DepartureCode="YUL", DepartureDetail="Montreal", ArrivalCode="YYZ",ArrivalDetail="Toronto", Day =1, OrderCapacity=orderCapacity},
                new Flight(){FlightId=2, DepartureCode="YUL", DepartureDetail="Montreal", ArrivalCode="YYC",ArrivalDetail="Calgary", Day =1, OrderCapacity=orderCapacity},
                new Flight(){FlightId=3, DepartureCode="YUL", DepartureDetail="Montreal", ArrivalCode="YVR",ArrivalDetail="Vancouver", Day =1, OrderCapacity=orderCapacity},
                new Flight(){FlightId=4, DepartureCode="YUL", DepartureDetail="Montreal", ArrivalCode="YYZ",ArrivalDetail="Toronto", Day =2, OrderCapacity=orderCapacity},
                new Flight(){FlightId=5, DepartureCode="YUL", DepartureDetail="Montreal", ArrivalCode="YYC",ArrivalDetail="Calgary", Day =2, OrderCapacity=orderCapacity},
                new Flight(){FlightId=6, DepartureCode="YUL", DepartureDetail="Montreal", ArrivalCode="YVR",ArrivalDetail="Vancouver", Day =2, OrderCapacity=orderCapacity},
            };

        return flights;
    }


    static Orders LoadOrdersFromJson(string fileName)
    {
        Orders orders = new Orders();
        orders.OrderList = new Dictionary<string, OrderDetail>();
        using (StreamReader streamReader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), fileName)))
        {

            string json = streamReader.ReadToEnd();
            JObject jObject = JObject.Parse(json);
            foreach (var order in jObject)
            {
                string orderId = order.Key;
                string destination = string.Empty;
                JToken orderToken = order.Value;
                if (orderToken is not null)
                {
                    destination = orderToken["destination"]?.ToObject<string>();
                }
                orders.OrderList.Add(orderId, new OrderDetail { Destination = destination });
            }
        }
        return orders;
    }

    static Dictionary<string, string> GenerateFlightOrders(List<Flight> flights, Orders orders)
    {
        var flightItineraries = new Dictionary<string, string>();
        foreach (var order in orders.OrderList)
        {
            bool orderScheduled = false;
            var availableFlight = flights.Where(x => order.Value.Destination == x.ArrivalCode);
            var hasFlightCapacity = availableFlight?.OrderBy(x => x.Day).FirstOrDefault(x => x.HasOrderCapacity);
            if (!availableFlight.Any())
            {
                flightItineraries.Add(order.Key, $"Flight is not scheduled for {order.Value.Destination}");
            }
            else if (hasFlightCapacity is not null)
            {
                if (hasFlightCapacity.OrderCapacity > 0)
                {
                    hasFlightCapacity.OrderCapacity--;
                }
                //flightItineraries.Add(order.Key, $"FlightNumber: {hasFlightCapacity.FlightId}, Departure: {hasFlightCapacity.DepartureDetail} ({hasFlightCapacity.DepartureCode}), Arrival: {hasFlightCapacity.ArrivalDetail} ({hasFlightCapacity.ArrivalCode}), day: {hasFlightCapacity.Day}, Remainin Capacity :{hasFlightCapacity.OrderCapacity}");
                flightItineraries.Add(order.Key, $"FlightNumber: {hasFlightCapacity.FlightId}, Departure: {hasFlightCapacity.DepartureDetail} ({hasFlightCapacity.DepartureCode}), Arrival: {hasFlightCapacity.ArrivalDetail} ({hasFlightCapacity.ArrivalCode}), day: {hasFlightCapacity.Day}");
                }
            else
            {
                flightItineraries.Add(order.Key, $"Flight is Full for {order.Value.Destination}");
            }
        }
        return flightItineraries;
    }

    static void DisplayFlightOrders(Dictionary<string, string> flightOrders)
    {
        Console.WriteLine("Generated Flight Itineraries:");
        foreach (var item in flightOrders)
        {
            Console.WriteLine($"Order: {item.Key}, {item.Value}");
        }
    }
}