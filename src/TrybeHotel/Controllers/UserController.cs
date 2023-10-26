using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers()
        {
            IEnumerable<UserDto> response = _repository.GetUsers();

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            if (_repository.UserEmailExists(user.Email)) return Conflict(new { message = "User email already exists" });
            UserDto response = _repository.Add(user);

            return Created("", response);
        }
    }
}