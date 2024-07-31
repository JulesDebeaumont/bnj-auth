namespace Server.Models;

public class Subscription
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long OrganizationId { get; set; }
    public long ApplicationId { get; set; }
    public long OrderId { get; set; }
    public required DateTime Expiration { get; set; }
    public bool Cancaled { get; set; } = false;
}