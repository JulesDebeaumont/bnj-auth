namespace Server.Models;

public class Quotation
{
    public long Id { get; set; }
    public long OrganizationId { get; set; }
    public long QuotationFileId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<QuotationContent> Contents { get; set; } = [];

    public class QuotationContent
    {

    }
}