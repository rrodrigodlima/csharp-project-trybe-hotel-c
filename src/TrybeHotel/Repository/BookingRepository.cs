using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var room = _context.Rooms
      .Include(r => r.Hotel)
      .ThenInclude(h => h.City)
      .FirstOrDefault(r => r.RoomId == booking.RoomId);

            if (room == null)
            {
                throw new KeyNotFoundException("Room not found");
            }

            var newBooking = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            return new BookingResponse
            {
                BookingId = newBooking.BookingId,
                CheckIn = newBooking.CheckIn,
                CheckOut = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = room.Hotel.HotelId,
                        Name = room.Hotel.Name,
                        Address = room.Hotel.Address,
                        CityId = room.Hotel.City.CityId,
                        CityName = room.Hotel.City.Name,
                        State = room.Hotel.City.State
                    }
                }
            };
        }
        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var findBooking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);

            if (findBooking == null)
            {
                throw new KeyNotFoundException("Booking not found");
            }

            if (user.UserId != findBooking.UserId)
            {
                throw new Exception("This Booking doesn't belong to this user");
            }

            var bookingResponse = (from booking in _context.Bookings
                                   where booking.BookingId == bookingId
                                   select new BookingResponse
                                   {
                                       BookingId = booking.BookingId,
                                       CheckIn = booking.CheckIn,
                                       CheckOut = booking.CheckOut,
                                       GuestQuant = booking.GuestQuant,
                                       Room = (from room in _context.Rooms
                                               where room.RoomId == booking.RoomId
                                               select new RoomDto
                                               {
                                                   Capacity = room.Capacity,
                                                   RoomId = room.RoomId,
                                                   Image = room.Image,
                                                   Name = room.Name,
                                                   Hotel = (from hotel in _context.Hotels
                                                            where hotel.HotelId == room.HotelId
                                                            select new HotelDto
                                                            {
                                                                Address = hotel.Address,
                                                                CityId = hotel.CityId,
                                                                HotelId = hotel.HotelId,
                                                                Name = hotel.Name,
                                                                CityName = (from city in _context.Cities
                                                                            where city.CityId == hotel.CityId
                                                                            select city.Name).First()
                                                            }).First()
                                               }).First()
                                   }).ToList();

            return bookingResponse.First();
        }

        public Room GetRoomById(int RoomId)
        {
            Room room = _context.Rooms!.FirstOrDefault(r => r.RoomId == RoomId) ?? throw new Exception("Quarto não encontrado");

            return room;
        }
        public object GetUserByEmail(string? name)
        {
            throw new NotImplementedException();
        }
    }

}