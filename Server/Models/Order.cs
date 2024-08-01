using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class Order
{
    [Key]
    public long Id {get; set;}
    public float PricePerSubscription { get; set;}
    public int SubscriptionCount { get; set;}
    public float TotalPrice { get => SubscriptionCount * PricePerSubscription; }
    public DateTime CreatedAt { get; set;} = DateTime.Now;


    public long? SubscriptionId { get; set;}
    public Subscription? Subscription { get; set;}


    [Required]
    public long OrganizationId { get; set;}
    public Organization? Organization { get; set;}


    [Required]
    public long ApplicationId { get; set;}
    public Application? Application { get; set;}


    [Required]
    public long QuotationId { get; set;}
    public Quotation? Quotation { get; set;}

}