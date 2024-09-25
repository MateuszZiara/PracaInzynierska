using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;
using PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;

namespace PracaInzynierska_RentIt.Server.Controllers.AspNetUsersEntity;
[Route("api/[controller]")]
[ApiController]
public class AspNetUsersController : ApplicationIdentityController<AspNetUsers, AspNetUsersService, AspNetUsersRepository, AspNetUsersResponseDTO>
{
    private readonly IAspNetUsersService _aspNetUsersServices;
    public AspNetUsersController(AspNetUsersService aspNetUsersServices) : base(aspNetUsersServices)
    {
        _aspNetUsersServices = aspNetUsersServices;
    }
    [HttpGet("checkEmail")]
    public bool CheckEmail(String email)
    {
        return _aspNetUsersServices.CheckEmail(email);
    }
    
    [HttpPost("Register")]
    public Task<ActionResult<AspNetUsers>> Register([FromBody] AspNetUsersRegisterDto user) => _aspNetUsersServices.Register(user);

    [HttpGet("info")]
    public async Task<AspNetUsersResponseDTO> GetUserInfo() => await _aspNetUsersServices.GetUserInfo();
    
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        foreach (var cookie in Request.Cookies.Keys)
        {
            if (cookie.StartsWith(".AspNetCore.Identity.Application"))
            {
                Response.Cookies.Delete(cookie);
            }
        }
        return Ok("Identity cookies deleted successfully.");
    }
    
    [HttpPost("Edit")]
    public bool Edit([FromBody] AspNetUsersEditDTO edit) => _aspNetUsersServices.Edit(edit);

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token) =>
        await _aspNetUsersServices.ConfirmEmail(userId, token);
    [HttpPost("SendConfirmation")]
    public async Task<bool> SendConfirmationEmail() => await _aspNetUsersServices.SendConfirmationEmail();

}


