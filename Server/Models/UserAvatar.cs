using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class UserAvatar
{
  [Key]
  public long Id { get; set; }
  public required string Filename { get; set; }
  public required string StorageFilename { get; set; }
  public string MimeType { get; set; } = "application/octet-stream";
  public DateTime CreatedAt { get; set; } = DateTime.Now;


  [Required]
  public long UserId { get; set; }
  public User? User { get; set; }

}
