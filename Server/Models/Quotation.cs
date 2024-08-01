using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class Quotation
{
  [Key]
  public long Id { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.Now;

  public ICollection<Order> Orders { get; set; } = [];

  public long? OrganizationId { get; set; }
  public Organization? Organization { get; set;}


  [Required]
  public long QuotationFileId { get; set; }
  public QuotationFile? QuotationFile { get; set;}

  public ICollection<QuotationContent> Contents { get; set; } = [];

  public class QuotationContent
  {

  }
}