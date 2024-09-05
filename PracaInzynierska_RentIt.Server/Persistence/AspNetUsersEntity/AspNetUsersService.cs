using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;

public class AspNetUsersService : IAspNetUsersService
{
    private readonly AspNetUsersRepository _aspNetUsersRepository;
    public AspNetUsersService(AspNetUsersRepository aspNetUsersRepository)
    {
        this._aspNetUsersRepository = aspNetUsersRepository;
    }
    public List<AspNetUsers> GetAll()
    {
        throw new NotImplementedException();
    }

    public ActionResult<AspNetUsers> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public ActionResult<AspNetUsers> Create(AspNetUsers t)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool Edit(AspNetUsersEditDTO newEntity)
    {
        var user = _aspNetUsersRepository.GetUserInfo();
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
        user.ModifiedTime = DateTime.Now;
        user.ModifiedBy = user.Email;
        return _aspNetUsersRepository.UpdateUser(user);
    }

    public bool CheckEmail(String email)
    {
        if (email == null)
            throw new Exception("There is problem with your email. Contact with the administrator");
        return _aspNetUsersRepository.CheckEmail(email);
    }

    public ActionResult<AspNetUsers> Register([FromBody] AspNetUsersRegisterDto users)
    {
        var passwordHasher = new PasswordHasher<AspNetUsers>();
        string hashedPassword = passwordHasher.HashPassword(null, users.Password);
        users.Password = hashedPassword;
        return _aspNetUsersRepository.Register(users);
    }

    public async Task<AspNetUsersResponseDTO> GetUserInfo() => new AspNetUsersResponseDTO().toDto(_aspNetUsersRepository.GetUserInfo());
    
}