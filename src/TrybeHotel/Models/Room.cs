namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class Room
{
  [Key]
  public int RoomId { get; set; }
  public string Name { get; set; } = string.Empty;
  public int Capacity { get; set; }
  public string Image { get; set; } = string.Empty;
  public int HotelId { get; set; }
  [ForeignKey("HotelId")]
  public Hotel? Hotel { get; set; }
  [InverseProperty("Room")]
  public ICollection<Booking>? Bookings { get; set; }
}