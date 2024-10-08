using System.ComponentModel;
using System.Net;
using System.Security.Claims;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Event.Default;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Email;

namespace PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;

public class AspNetUsersRepository : IAspNetUsersRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AspNetUsers> _userManager;
    private readonly EmailService _emailService;
    private readonly SignInManager<AspNetUsers> _signInManager;
    public AspNetUsersRepository(UserManager<AspNetUsers> userManager, EmailService emailService, IHttpContextAccessor httpContextAccessor,  SignInManager<AspNetUsers> signInManager)
    {
        _userManager = userManager;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
    }
    public AspNetUsersResponseDTO ConvertToDto(AspNetUsers user)
    {
        return new AspNetUsersResponseDTO
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDateTime = user.BirthDate,
            UserRank = user.UserRank,
            EmailConfirmed = user.EmailConfirmed
        };
    }
    public AspNetUsers Edit(AspNetUsersEditDTO newEntity)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                var old = session.Query<AspNetUsers>().FirstOrDefault(x => x.Id == newEntity.Id);
                if (old == null)
                {
                    throw new Exception("Internal error while trying to edit your account");
                }
                return old;
            }
        }
    }
    public bool UpdateUser(AspNetUsers user)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.SaveOrUpdate(user);
                    transaction.Commit();
                    session.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
    public bool CheckEmail(string email)
    {
        var user = NHibernateHelper.OpenSession().Query<AspNetUsers>().Where(x => x.Email == email).ToList();
        if (user.Count == 0)
            return false;
        return true;
    }

    public async Task<bool> sendRegisterEmail(AspNetUsers final)
    {
        var emailSubject = "Witaj w RentIt!";
        var emailBody = $@"
                        <html>
                        <body style='font-family: Arial, sans-serif; text-align: center; color: #333;'>
                            <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                                <h2 style='color: #4CAF50;'>Witaj w RentIt {final.FirstName},</h2>
                                <p>Pomyślnie zarejestrowałeś się w serwisie RentIt.:</p>
                                <p style='margin-top: 30px;'>Twoim następnym krokiem jest znalezienie nowych współlokatorów, z którymi mamy nadzieję spędzisz niezapomniane momenty.</p>
                                <p style='margin-top: 30px;'>Życzymy ci wszystkiego dobrego. Zespół RentIt!</p>
                                <p style='margin-top: 30px; font-size: 12px; color: #999;'>© 2024 RentIt. All rights reserved.</p>
                            </div>
                        </body>
                        </html>";
        try
        {
            await _emailService.SendEmailAsync(final.Email, emailSubject, emailBody);
            return true;
        }
        catch
        {
            throw new Exception("Can't send email. Email service is unavaible. Please contanct with support.");
        }
    }
    public async Task<bool> SendConfirmationEmail()
    {
        var final = GetUserInfo();
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(final);
        var confirmationLink =
            $"https://localhost:7214/api/AspNetUsers/ConfirmEmail?userId={final.Id}&token={WebUtility.UrlEncode(token)}";
        var emailSubject = "Confirm your email";
        var emailBody = $@"
                        <html>
                        <body style='font-family: Arial, sans-serif; text-align: center; color: #333;'>
                            <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                                <h2 style='color: #4CAF50;'>Potwierdź swój email</h2>
       
                                <p>Aby potwierdzić swój email kliknij w przycisk poniżej:</p>
                                <a href='{confirmationLink}' 
                                   style='display: inline-block; margin-top: 20px; padding: 10px 20px; 
                                          background-color: #4CAF50; color: white; text-decoration: none; 
                                          border-radius: 5px; font-weight: bold;'>
                                    Potwierdź Email
                                </a>
                                <p style='margin-top: 30px;'>Jeżeli to nie jest twoje konto istnieje duże prawdopodobieństwo, że ktoś użytwa twoich danych. Zgłoś to do administracji RentIt w centrum pomocy.</p>
                                <p style='margin-top: 30px; font-size: 12px; color: #999;'>© 2024 RentIt. All rights reserved.</p>
                            </div>
                        </body>
                        </html>";
        try
        {
            await _emailService.SendEmailAsync(final.Email, emailSubject, emailBody);
            return true;
        }
        catch
        {
            throw new Exception("Can't send email. Email service is unavaible. Please contanct with support.");
        }
    }

    public void ModifiedUpdate(AspNetUsers user, string modifier)
    {
        var session = NHibernateHelper.OpenSession();
        var transaction = session.BeginTransaction();
        user.ModifiedBy = modifier;
        user.ModifiedTime = DateTime.Now;
        session.SaveOrUpdate(user);
        transaction.Commit();
    }
    public async Task<bool> ResetPassword(AspNetUsersPasswordDTO passwordDto)
    {   
        
        var currentUser = await _userManager.FindByIdAsync(GetUserInfo().Id);
        if (currentUser == null)
        {
            return false;
        }

        var passwordCheck = await _signInManager.CheckPasswordSignInAsync(currentUser, passwordDto.OldPassword, false);
        if (!passwordCheck.Succeeded)
        {
            return false;
        }

        var result = await _userManager.ChangePasswordAsync(currentUser, passwordDto.OldPassword, passwordDto.NewPassword);
        if (result.Succeeded)
        {
            ModifiedUpdate(currentUser,"User");
            return true;
        }

        foreach (var error in result.Errors)
        {
            Console.WriteLine(error.Description);
        }

        return false;
    }



    public async Task<bool> Logout()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            foreach (var cookie in httpContext.Request.Cookies.Keys)
            {
                if (cookie.StartsWith(".AspNetCore.Identity.Application"))
                {
                    httpContext.Response.Cookies.Delete(cookie);
                }
            }
            return true;
        }
        return false;
    }

    public async Task<ActionResult<AspNetUsers>> Register(AspNetUsersRegisterDto user)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                AspNetUsers final = new AspNetUsers().ToEntity(user);
                final.NormalizedEmail = user.Email.ToUpper();
                final.UserName = user.Email;
                final.NormalizedUserName = final.UserName.ToUpper();
                session.Save(final);
                transaction.Commit();
                session.Close();
                await sendRegisterEmail(final);
                return final.ToEntity(user);
            }
        }
    }
    public AspNetUsers GetUserInfo()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity != null && user.Identity.IsAuthenticated)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new Exception("User ID claim not found.");
            }
            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new Exception("Invalid user ID format.");
            }
            using (var session = NHibernateHelper.OpenSession())
            {
                var userEntity = session.Get<AspNetUsers>(userIdClaim.Value);
                if (userEntity == null)
                {
                    throw new ObjectNotFoundException(userEntity, "Can't find user");
                }

                return userEntity;
            }
        }
        throw new UnauthorizedAccessException();
    }
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("Can't find user");
        }

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return new RedirectResult("https://localhost:5173/confirm");
        }

        throw new Exception("Error confirming Email");
    }
    
}