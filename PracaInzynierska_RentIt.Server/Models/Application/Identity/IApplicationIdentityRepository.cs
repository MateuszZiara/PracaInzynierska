using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public interface IApplicationIdentityRepository<T, TDto> where T : IdentityUser
{
    public TDto ConvertToDto(T? entity);
    public List<TDto> GetAll()
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            List<TDto> items = new List<TDto>();
            foreach (var item in session.Query<T>())
            {
                items.Add(ConvertToDto(item));
            }

            session.Close();
            return items;
        }
    }
    public ActionResult<TDto> GetById(Guid id) =>ConvertToDto(NHibernateHelper.OpenSession().Query<T>().FirstOrDefault(x => x.Id == id.ToString())) ?? throw new InvalidOperationException();

    public ActionResult<TDto> Create(T t)
    {
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(t);
                transaction.Commit();
                session.Close();
                return ConvertToDto(t);
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