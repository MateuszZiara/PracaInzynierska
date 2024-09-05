namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

public class AspNetUsersEditDTO
{
    public virtual string? Id { get; set; }
    public virtual string? FirstName { get; set; }
    public virtual string? LastName { get; set; }
    public virtual DateTime? BirthDay { get; set; }
    public virtual string? Password { get; set; }
    public virtual string? Email { get; set; }
    public virtual string? PhoneNumber { get; set; }
    public virtual bool? Avatar { get; set; }
    public virtual bool? TwoFactorEnabled { get; set; }
    public virtual List<string>? Lokalizacja { get; set; }
}