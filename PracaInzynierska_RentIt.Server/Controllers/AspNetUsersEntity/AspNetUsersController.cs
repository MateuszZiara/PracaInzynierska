﻿using Microsoft.AspNetCore.Mvc;
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
}


