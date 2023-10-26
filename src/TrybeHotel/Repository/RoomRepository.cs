using TrybeHotel.Models;
using TrybeHotel.Dto;

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
            IEnumerable<RoomDto> rooms = _context.Rooms!.Where(r => r.HotelId == HotelId).Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Image = r.Image,
                Hotel = new HotelDto
                {
                    HotelId = HotelId,
                    Name = r.Hotel!.Name,
                    Address = r.Hotel.Address,
                    CityId = r.Hotel.CityId,
                    CityName = r.Hotel.City!.Name
                }
            });

            return rooms;
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room)
        {
            // Adiciona o novo quarto ao contexto
            _context.Rooms.Add(room);
            _context.SaveChanges();

            Hotel? hotel = _context.Hotels!.FirstOrDefault(h => h.HotelId == room.HotelId);

            string? cityName = _context.Cities!.FirstOrDefault(c => c.CityId == hotel!.CityId)!.Name;

            return new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = new HotelDto
                {
                    HotelId = room.HotelId,
                    Name = hotel!.Name,
                    Address = hotel.Address,
                    CityId = hotel.CityId,
                    CityName = cityName
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