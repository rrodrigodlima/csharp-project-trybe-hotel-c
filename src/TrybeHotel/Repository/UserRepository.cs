using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            User user = _context.Users!.FirstOrDefault(u => u.UserId == userId) ?? throw new Exception("Usuário não encontrado");

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };
        }

        public UserDto Login(LoginDto login)
        {
            var users = _context.Users;
            var user = users.FirstOrDefault(user => user.Password == login.Password && user.Email == login.Email);
            if (user == null) return null;

            var response = new UserDto()
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };
            return response;
        }
        public UserDto Add(UserDtoInsert user)
        {
            int? lastId = _context.Users!.OrderBy(u => u.UserId).LastOrDefault()?.UserId;
            int newId = (int)(lastId == null ? 1 : lastId + 1);
            User newUser = new()
            {
                UserId = newId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
            };

            _context.Users!.Add(newUser);
            _context.SaveChanges();

            return new UserDto
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            User user = _context.Users!.FirstOrDefault(u => u.Email == userEmail) ?? throw new Exception("Usuário não encontrado");

            var data = new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                UserType = user.UserType
            };

            return data;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return _context.Users!.Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            });
        }

        public bool UserEmailExists(string userEmail)
        {
            return _context.Users!.Any(u => u.Email == userEmail);
        }
    }
}