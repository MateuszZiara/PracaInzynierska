using System.ComponentModel;
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
}