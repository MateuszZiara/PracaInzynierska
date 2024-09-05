using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;
using RentIt_PracaInzynierska.Server.Models.AspNetUsersEntity;

namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;

public class AspNetUsers : IdentityUser
{
    public AspNetUsers() 
    {
        Avatar = false;
        ModifiedTime = null;
        ModifiedBy = null;
        CreateTime = DateTime.Now;
        UserRank = UserRank.User;
        CreateBy = "Admin";
    }

    public AspNetUsers(string firstName, string lastName, string provider, string password, string email, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        Provider = provider;
        Email = email;
        PasswordHash = password;
        Avatar = false;
        ModifiedTime = null;
        ModifiedBy = null;
        CreateTime = DateTime.Now;
        UserRank = UserRank.User;
        BirthDate = birthDate;
        CreateBy = provider;
    }

    

    public virtual AspNetUsers ToEntity(AspNetUsersRegisterDto user)
    {
        return new AspNetUsers(
            user.FirstName,
            user.LastName,
            user.Provider,
            user.Password,
            user.Email,
            user.BirthDay);
    }

    public virtual AspNetUsersResponseDTO ToResponse()
    {
        return new AspNetUsersResponseDTO(FirstName, LastName, BirthDate, UserRank, EmailConfirmed);
    }
    public virtual string? FirstName { get; set; }
    public virtual string? LastName { get; set; }
    public virtual UserRank UserRank { get; set; }
    public virtual string? Provider { get; set; }
    public virtual string? Name { get; set; } 
    public virtual bool? Avatar {get; set; }
    public virtual DateTime? ModifiedTime { get; set; }
    public virtual String? ModifiedBy { get; set; }
    [Required(ErrorMessage = "Pole CreateTime nie może być puste")]
    public virtual DateTime CreateTime { get; set; }
    [Required(ErrorMessage = "Pole CreateBy nie może być puste")]
    public virtual String CreateBy { get; set; }
    public virtual DateTime BirthDate { get; set; }
}