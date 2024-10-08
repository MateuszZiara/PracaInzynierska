using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;
using PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;

namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;

public interface IAspNetUsersService : IApplicationIdentityService<AspNetUsers, IAspNetUsersRepository, AspNetUsersResponseDTO>
{
    public bool Edit(AspNetUsersEditDTO newEntity);
    public bool CheckEmail(String email);
    public Task<ActionResult<AspNetUsers>> Register([FromBody] AspNetUsersRegisterDto users);
    public Task<AspNetUsersResponseDTO> GetUserInfo();
    public Task<IActionResult> ConfirmEmail(string userId, string token);
    public Task<bool> SendConfirmationEmail();

    public Task<bool> ResetPassword(AspNetUsersPasswordDTO passwordDto);
}