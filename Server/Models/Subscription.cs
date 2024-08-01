using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class Subscription
{
  [Key]
  public long Id { get; set; }
  public required DateTime Expiration { get; set; }
  public bool Canceled { get; set; } = false;


  [Required]
  public long UserId { get; set; }
  public User? User { get; set; }


  [Required]
  public long OrganizationId { get; set; }
  public Organization? Organization { get; set; }


  [Required]
  public long ApplicationId { get; set; }
  public Application? Application { get; set; }

  public ICollection<Order> Orders { get; set; } = [];
}
