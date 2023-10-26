namespace TrybeHotel.Models;

using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum UserTypes
{
    admin,
    client
};

public class User
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    private UserTypes _userType = UserTypes.client;
    public string UserType
    {
        get => _userType.ToString();
        set
        {
            _userType = value switch
            {
                "admin" => UserTypes.admin,
                "client" => UserTypes.client,
                _ => throw new Exception("Tipo de usuário inválido"),
            };
        }
    }
    [InverseProperty("User")]
    public ICollection<Booking>? Bookings { get; set; }
};