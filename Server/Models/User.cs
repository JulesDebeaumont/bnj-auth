namespace Server.Models;

public class User
{
    public long Id { get; set; }
    public long AvatarId { get; set; }
    public long OrganizationId { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
}
