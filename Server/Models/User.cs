using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class User
{
    [Key]
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }


    [Required]
    public long OrganizationId { get; set; }
    public Organization? Organization { get; set;}


    public long? UserAvatarId { get; set; }
    public UserAvatar? UserAvatar { get; set;}


    public ICollection<Subscription> Subscriptions { get; set;} = [];

}
