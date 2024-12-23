using System.Globalization;
using HotelAvailabilityChecker.Models;

namespace HotelAvailabilityChecker.Services
{
    public class AvailabilityChecker
    {
        public static int CheckAvailability(
            List<Hotel> hotels,
            List<Booking> bookings,
            string hotelId,
            string roomType,
            DateTime startDate,
            DateTime endDate)
        {

            var hotel = hotels.Find(h => h.Id == hotelId);
            if (hotel == null)
            {
                Console.WriteLine("Invalid Hotel ID.");
                return 0;
            }

            int totalRooms = hotel.Rooms.FindAll(r => r.RoomType == roomType).Count;
            if (totalRooms == 0)
            {
                Console.WriteLine("Invalid Room Type.");
                return 0;
            }


            var overlappingBookings = bookings.FindAll(b =>
{
    try
    {

        string[] formats = { "yyyyMMdd", "yyyy-MM-dd" };
        DateTime bookingStart = DateTime.ParseExact(b.Arrival, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        DateTime bookingEnd = DateTime.ParseExact(b.Departure, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);

        return b.HotelId == hotelId &&
               b.RoomType == roomType &&
               startDate < bookingEnd &&
               endDate > bookingStart;
    }
    catch (FormatException)
    {
        Console.WriteLine($"Invalid date format in booking data: {b.Arrival} - {b.Departure}");
        return false;
    }
});


            int bookedRooms = overlappingBookings.Count;
            int availableRooms = totalRooms - bookedRooms;
            return availableRooms;
        }
    }
}
