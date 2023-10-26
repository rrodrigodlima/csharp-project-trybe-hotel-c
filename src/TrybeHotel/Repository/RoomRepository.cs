using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var rooms = _context.Rooms
                .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                .Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    Name = r.Name,
                    Capacity = r.Capacity,
                    Image = r.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = r.Hotel.HotelId,
                        Name = r.Hotel.Name,
                        Address = r.Hotel.Address,
                        CityId = r.Hotel.CityId,
                        CityName = r.Hotel.City.Name,
                        State = r.Hotel.City.State
                    }
                })
                .ToList();

            return rooms;
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room)
        {
            var hotel = _context.Hotels
           .Include(h => h.City)
           .FirstOrDefault(h => h.HotelId == room.Hotel.HotelId);

            if (hotel == null)
            {
                throw new KeyNotFoundException("Hotel not found");
            }

            var newRoom = new Room
            {
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                HotelId = room.Hotel.HotelId
            };

            _context.Rooms.Add(newRoom);
            _context.SaveChanges();

            City city = _context.Cities!.FirstOrDefault(c => c.CityId == hotel!.CityId) ?? throw new Exception("Cidade não encontrada");

            return new RoomDto
            {
                RoomId = newRoom.RoomId,
                Name = newRoom.Name,
                Capacity = newRoom.Capacity,
                Image = newRoom.Image,
                Hotel = new HotelDto
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = hotel.City.CityId,
                    CityName = city.Name,
                    State = city.State
                }
            };
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        public void DeleteRoom(int RoomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId); if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }
    }
}