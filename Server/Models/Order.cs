namespace Server.Models;

public class Order
{
    public long Id {get; set;}
    public float PricePerSubscription { get; set;}
    public float TotalPrice { get; set;}
    public int SubscriptionCount { get; set;}
    public DateTime CreatedAt { get; set;} = DateTime.Now;
    public long SubscriptionId { get; set;}
    public long OrganizationId { get; set;}
    public long ApplicationId { get; set;}
    public long QuotationId { get; set;}
}