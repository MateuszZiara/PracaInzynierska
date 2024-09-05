using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public interface IApplicationRepository<T>
{
    public List<T> GetAll();
    public ActionResult<T> GetById(Guid id);
    public ActionResult<T> Create(T t);
    public bool Delete(Guid id);
    public T Edit(AspNetUsersEditDTO newEntity);
}