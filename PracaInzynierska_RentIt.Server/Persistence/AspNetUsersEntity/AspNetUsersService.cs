using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;

public class AspNetUsersService : IAspNetUsersService
{
    public IAspNetUsersRepository Repository { get; }
    public AspNetUsersService(AspNetUsersRepository aspNetUsersRepository)
    {
        this.Repository = aspNetUsersRepository;
    }
    
    public bool Edit(AspNetUsersEditDTO newEntity)
    {
        var user = Repository.GetUserInfo();
        if (newEntity.FirstName != null)
            user.FirstName = newEntity.FirstName;
        if (newEntity.LastName != null)
            user.LastName = newEntity.LastName;
        if (newEntity.Password != null)
        {
            var passwordHasher = new PasswordHasher<AspNetUsers>();
            string hashedPassword = passwordHasher.HashPassword(null, newEntity.Password);
            user.PasswordHash = hashedPassword;
        }

        if (newEntity.Email != null)
        {
            user.Email = newEntity.Email;
            user.EmailConfirmed = false;
        }

        if (newEntity.PhoneNumber != null)
        {
            user.PhoneNumber = newEntity.PhoneNumber;
            user.PhoneNumberConfirmed = false;
        }

        if (newEntity.Avatar != null)
            user.Avatar = newEntity.Avatar;
        if (newEntity.TwoFactorEnabled != null)
            user.TwoFactorEnabled = (bool)newEntity.TwoFactorEnabled;
        /*user.ModifiedTime = DateTime.Now;
        user.ModifiedBy = user.Email;*/
        Repository.ModifiedUpdate(user,"User");
        return Repository.UpdateUser(user);
    }

    public bool CheckEmail(String email)
    {
        if (email == null)
            throw new Exception("There is problem with your email. Contact with the administrator");
        return Repository.CheckEmail(email);
    }

    public Task<ActionResult<AspNetUsers>> Register([FromBody] AspNetUsersRegisterDto users)
    {
        var passwordHasher = new PasswordHasher<AspNetUsers>();
        string hashedPassword = passwordHasher.HashPassword(null, users.Password);
        users.Password = hashedPassword;
        return Repository.Register(users);
    }   
    
    public async Task<AspNetUsersResponseDTO> GetUserInfo() => new AspNetUsersResponseDTO().toDto(Repository.GetUserInfo());

    public async Task<IActionResult> ConfirmEmail(string userId, string token) => await Repository.ConfirmEmail(userId,token);
    public async Task<bool> SendConfirmationEmail() => await Repository.SendConfirmationEmail();

    public async Task<bool> ResetPassword(AspNetUsersPasswordDTO passwordDto)
    {
        bool result = await Repository.ResetPassword(passwordDto);
        if (result)
        {
            await Repository.Logout();
            return true;
        }

        return false;
    }
}