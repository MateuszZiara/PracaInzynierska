using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public interface IApplicationService<T, TD, TR> where TR : IApplicationRepository<T,TD> where T : ApplicationEntity
{
    TR Repository { get; }
    public List<T> GetAll() => Repository.GetAll();
    public ActionResult<T> GetById(Guid id) => Repository.GetById(id);
    public ActionResult<T> Create(T t) => Repository.Create(t);
    public bool Delete(Guid id) => Repository.Delete(id);
}