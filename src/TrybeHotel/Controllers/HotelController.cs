using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("hotel")]
    public class HotelController : Controller
    {
        private readonly IHotelRepository _repository;

        public HotelController(IHotelRepository repository)
        {
            _repository = repository;
        }

        // 4. Desenvolva o endpoint GET /hotel
        [HttpGet]
        public IActionResult GetHotels()
        {
            // Chama o método GetHotels() do repositório
            IEnumerable<HotelDto> hotels = _repository.GetHotels();

            return Ok(hotels);
        }

        // 5. Desenvolva o endpoint POST /hotel
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult PostHotel([FromBody] Hotel hotel)
        {
            // Chame o método AddHotel() do repositório para inserir o hotel
            HotelDto addedHotel = _repository.AddHotel(hotel);

            return CreatedAtAction(nameof(GetHotels), addedHotel);
        }


    }
}