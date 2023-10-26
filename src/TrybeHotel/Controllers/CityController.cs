using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("city")]
    public class CityController : Controller
    {
        private readonly ICityRepository _repository;
        public CityController(ICityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            IEnumerable<CityDto> cities = _repository.GetCities();

            return Ok(cities);
        }

        [HttpPost]
        public IActionResult PostCity([FromBody] City city)
        {
            CityDto addedCity = _repository.AddCity(city);

            return Created("", addedCity);
        }

        // 3. Desenvolva o endpoint PUT /city
        [HttpPut]
        public IActionResult PutCity([FromBody] City city)
        {
            try
            {
                CityDto updatedCity = _repository.UpdateCity(city);

                return Ok(updatedCity);
            }
            catch (Exception e)
            {
                
                return BadRequest(new { message = e.Message });
            }
        }
    }
}