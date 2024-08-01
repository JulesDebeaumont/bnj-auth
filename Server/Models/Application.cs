using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class Application
{
    [Key]
    public long Id { get; set;}

    [Required]
    public required string Name { get; set;}

    public ICollection<Order> Orders {get; set;} = [];
    public ICollection<Subscription> Subscriptions {get; set;} = [];
}
