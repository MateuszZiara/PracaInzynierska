using RentIt_PracaInzynierska.Server.Models.AspNetUsersEntity;

namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

public class AspNetUsersResponseDTO
{
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual DateTime BirthDateTime { get; set; }
    public virtual UserRank UserRank { get; set; }

    public AspNetUsersResponseDTO(string firstName, string lastName, DateTime birthDateTime, UserRank userRank)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDateTime = birthDateTime;
        UserRank = userRank;
    }
}