using NHibernate.Mapping;
using RentIt_PracaInzynierska.Server.Models.AspNetUsersEntity;

namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

public class AspNetUsersResponseDTO
{
    public AspNetUsersResponseDTO()
    {
    }

    public virtual string? FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual DateTime BirthDateTime { get; set; }
    public virtual UserRank UserRank { get; set; }
    public virtual bool EmailConfirmed { get; set; }
    public virtual string Email { get; set; }

    public AspNetUsersResponseDTO(string? firstName, string lastName, DateTime birthDateTime, UserRank userRank, bool emailConfirmed, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDateTime = birthDateTime;
        UserRank = userRank;
        EmailConfirmed = emailConfirmed;
        Email = email;
    }

    public AspNetUsersResponseDTO toDto(AspNetUsers user)
    {
        return new AspNetUsersResponseDTO(user.FirstName, user.LastName, user.BirthDate, user.UserRank,
            user.EmailConfirmed, user.Email);
    }
}