using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;

public class AspNetUsersRepository : IAspNetUsersRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AspNetUsersRepository(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
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
    public bool CheckEmail(string email)
    {
        var user = NHibernateHelper.OpenSession().Query<AspNetUsers>().Where(x => x.Email == email).ToList();
        if (user.Count == 0)
            return false;
        return true;
    }

    public ActionResult<AspNetUsers> Register(AspNetUsersRegisterDto user)
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
                return final.ToEntity(user);
            }
        }
    }
    public async Task<AspNetUsersResponseDTO> GetUserInfo()
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
                return userEntity.ToResponse();
            }
        }
        throw new UnauthorizedAccessException();
    }
}