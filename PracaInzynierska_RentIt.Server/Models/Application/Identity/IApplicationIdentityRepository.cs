using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public interface IApplicationIdentityRepository<T> where T : IdentityUser
{
    public List<T> GetAll() => NHibernateHelper.OpenSession().Query<T>().ToList();
    public ActionResult<T> GetById(Guid id) => NHibernateHelper.OpenSession().Query<T>().FirstOrDefault(x => x.Id == id.ToString()) ?? throw new InvalidOperationException();

    public ActionResult<T> Create(T t)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(t);
                transaction.Commit();
                session.Close();
                return t;
            }
        }
    }

    public bool Delete(Guid id)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var entity = session.Query<T>().Where(x => x.Id == id.ToString());
                    session.Delete(entity);
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
    
}