using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public interface IApplicationService<T,TR, TDto> where TR : IApplicationRepository<T, TDto> where T : ApplicationEntity
{
    TR Repository { get; }
    public List<TDto> GetAll() => Repository.GetAll();
    public ActionResult<TDto> GetById(Guid id) => Repository.GetById(id);
    public ActionResult<TDto> Create(T t) => Repository.Create(t);
    public bool Delete(Guid id) => Repository.Delete(id);
}