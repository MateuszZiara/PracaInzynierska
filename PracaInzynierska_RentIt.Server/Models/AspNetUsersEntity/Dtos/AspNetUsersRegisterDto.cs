using System.Runtime.InteropServices.JavaScript;

namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

public class AspNetUsersRegisterDto
{
    public virtual string Email { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Password { get; set; }
    public virtual string Provider { get; set; }
    public virtual DateTime BirthDay { get; set; }
}