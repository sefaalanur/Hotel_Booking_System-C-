using Newtonsoft.Json;
using HotelAvailabilityChecker.Models;
using HotelAvailabilityChecker.Services;

namespace HotelAvailabilityChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var hotels = LoadData<List<Hotel>>("Data/hotels.json");
            var bookings = LoadData<List<Booking>>("Data/bookings.json");

            if (hotels == null || bookings == null)
            {
                Console.WriteLine("Error loading data files. Please ensure 'hotels.json' and 'bookings.json' exist and are properly formatted.");
                return;
            }

            Console.WriteLine("Press Enter with an empty line to exit.");

            while (true)
            {
                Console.Write("Enter Hotel ID: ");
                string hotelId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(hotelId)) break;

                Console.Write("Enter Room Type (e.g., SGL, DBL): ");
                string roomType = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(roomType)) break;

                Console.Write("Enter Start Date (YYYYMMDD or YYYY-MM-DD): ");
                string startDateInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(startDateInput)) break;

                Console.Write("Enter End Date (YYYYMMDD or YYYY-MM-DD): ");
                string endDateInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(endDateInput)) break;

                
                try
                {
                    DateTime startDate = ParseDate(startDateInput);
                    DateTime endDate = ParseDate(endDateInput);

                    
                    int availableRooms = AvailabilityChecker.CheckAvailability(hotels, bookings, hotelId, roomType, startDate, endDate);
                    Console.WriteLine($"Available Rooms: {availableRooms}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Invalid date format: {ex.Message}");
                }
            }
        }

        private static T? LoadData<T>(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file {filePath}: {ex.Message}");
                return default;
            }
        }

        private static DateTime ParseDate(string input)
        {
            string[] formats = { "yyyyMMdd", "yyyy-MM-dd" };
            return DateTime.ParseExact(input, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
        }
    }
}
