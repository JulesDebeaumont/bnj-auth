
using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class Organization {
    [Key]
    public long Id { get; set; }
    public required string Name { get; set;}
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Contact { get; set;}

    public ICollection<Order> Orders { get; set;} = [];
    public ICollection<User> Users { get; set;} = [];
    public ICollection<Quotation> Quotations { get; set;} = [];
}
