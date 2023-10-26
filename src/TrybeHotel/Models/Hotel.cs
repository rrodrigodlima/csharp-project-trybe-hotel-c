namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class Hotel
{
  [Key]
  public int HotelId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public int CityId { get; set; }
  [InverseProperty("Hotel")]
  public ICollection<Room>? Rooms { get; set; }
  [ForeignKey("CityId")]
  public City? City { get; set; }
  // Propriedade de navegação para a cidade
}