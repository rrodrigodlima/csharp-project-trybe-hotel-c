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
            _context.Rooms.Add(room);
            _context.SaveChanges();

            var createdRoom = from rooms in _context.Rooms
                              where rooms.RoomId == room.RoomId
                              join hotel in _context.Hotels on rooms.HotelId equals hotel.HotelId
                              join city in _context.Cities on hotel.CityId equals city.CityId
                              select new RoomDto()
                              {
                                  RoomId = rooms.RoomId,
                                  Name = rooms.Name,
                                  Image = rooms.Image,
                                  Capacity = rooms.Capacity,
                                  Hotel = new HotelDto()
                                  {
                                      HotelId = hotel.HotelId,
                                      CityId = city.CityId,
                                      Address = hotel.Address,
                                      Name = hotel.Name,
                                      CityName = city.Name,
                                      State = hotel.City.State,
                                  }
                              };

            return createdRoom.First();
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