using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            try
            {
                var tokenIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var email = tokenIdentity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                if (email == null)
                {
                    return BadRequest(new { message = "Unable to extract email from token" });
                }

                var response = _repository.Add(bookingInsert, email);

                if (response == null)
                {
                    return BadRequest(new { message = "Guest quantity over room capacity" });
                }

                return Created("", response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }


        [HttpGet("{Bookingid}")]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid)
        {
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var response = _repository.GetBooking(Bookingid, email);
                
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}