using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;

public class AspNetUsersRepository : IAspNetUsersRepository
{
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
    
    
}