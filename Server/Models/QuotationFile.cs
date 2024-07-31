namespace Server.Models;

public class QuotationFile
{
    public long Id { get; set; }
    public required long QuotationId { get; set; }
    public required string Filename { get; set; }
    public required string StorageFilename { get; set; }
    public string MimeType { get; set; } = "application/octet-stream";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
