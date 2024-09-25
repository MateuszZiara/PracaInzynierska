using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;

public interface IAspNetUsersRepository : IApplicationIdentityRepository<AspNetUsers, AspNetUsersResponseDTO>
{
    public Task<ActionResult<AspNetUsers>> Register([FromBody] AspNetUsersRegisterDto users);
    public AspNetUsers GetUserInfo();
    public bool CheckEmail(string email);
    public bool UpdateUser(AspNetUsers user);
    public AspNetUsers Edit(AspNetUsersEditDTO newEntity);
}