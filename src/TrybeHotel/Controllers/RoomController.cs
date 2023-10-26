using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _repository;
        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        [HttpGet("{HotelId}")]
        public IActionResult GetRoom(int HotelId)
        {
            // Chama o método GetRooms() do repositório para obter a lista de quartos
            IEnumerable<RoomDto> rooms = _repository.GetRooms(HotelId);

            return Ok(rooms);
        }

        // 7. Desenvolva o endpoint POST /room
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult PostRoom([FromBody] Room room)
        {
            RoomDto response = _repository.AddRoom(room);

            return Created($"/room/{response.RoomId}", response);
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        [HttpDelete("{RoomId}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int RoomId)
        {
            // Chame o método DeleteRoom() do repositório para excluir o quarto
            _repository.DeleteRoom(RoomId);

            return NoContent();
        }
    }
}