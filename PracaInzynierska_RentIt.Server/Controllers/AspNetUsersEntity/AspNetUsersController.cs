using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;
using PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;

namespace PracaInzynierska_RentIt.Server.Controllers.AspNetUsersEntity;
[Route("api/[controller]")]
[ApiController]
public class AspNetUsersController : ControllerBase
{
    private readonly AspNetUsersService _aspNetUsersServices;
    public AspNetUsersController(AspNetUsersService aspNetUsersServices)
    {
        _aspNetUsersServices = aspNetUsersServices;
    }
    [HttpGet("checkEmail")]
    public bool CheckEmail(String email)
    {
        return _aspNetUsersServices.CheckEmail(email);
    }
    
    [HttpPost("Register")]
    public ActionResult<AspNetUsers> Register([FromBody] AspNetUsersRegisterDto user) => _aspNetUsersServices.Register(user);
}


