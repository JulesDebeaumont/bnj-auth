
namespace Server.Models;

public class Organization {
    public long Id { get; set; }
    public required string Name { get; set;}
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Contact { get; set;}
}
