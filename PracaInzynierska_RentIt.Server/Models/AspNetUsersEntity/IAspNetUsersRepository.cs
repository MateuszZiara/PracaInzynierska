using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;

public interface IAspNetUsersRepository : IApplicationIdentityRepository<AspNetUsers>
{
    
}