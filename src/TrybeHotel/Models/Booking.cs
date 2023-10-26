namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class Booking
{
  [Key]
  public int BookingId { get; set; }
  public DateTime CheckIn { get; set; }
  public DateTime CheckOut { get; set; }
  public int GuestQuant { get; set; }
  public int UserId { get; set; }
  public int RoomId { get; set; }
  [ForeignKey("UserId")]
  public User? User { get; set; }
  [ForeignKey("RoomId")]
  public Room? Room { get; set; }
}