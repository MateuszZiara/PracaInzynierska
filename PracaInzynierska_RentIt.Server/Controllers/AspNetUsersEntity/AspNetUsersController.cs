using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
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

    [HttpGet("/checkEmail")]
    public bool CheckEmail(String email) => _aspNetUsersServices.CheckEmail(email);

}