using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public interface IApplicationIdentityService<T, TR, TDto> where TR : IApplicationIdentityRepository<T, TDto> where T : IdentityUser
{
    TR Repository { get; }
    List<TDto> GetAll() => Repository.GetAll();
    ActionResult<TDto> GetById(Guid id) => Repository.GetById(id);
    ActionResult<TDto> Create(T t) => Repository.Create(t); 
    bool Delete(Guid id) => Repository.Delete(id);
}