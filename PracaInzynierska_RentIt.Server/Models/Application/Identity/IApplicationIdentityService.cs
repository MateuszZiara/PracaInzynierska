using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public interface IApplicationIdentityService<T, TR> where TR : IApplicationIdentityRepository<T> where T : IdentityUser
{
    TR Repository { get; }
    List<T> GetAll() => Repository.GetAll();
    ActionResult<T> GetById(Guid id) => Repository.GetById(id);
    ActionResult<T> Create(T t) => Repository.Create(t); 
    bool Delete(Guid id) => Repository.Delete(id);
    public bool Edit(AspNetUsersEditDTO newEntity);
    public bool CheckEmail(String email);
    public ActionResult<AspNetUsers> Register([FromBody] AspNetUsersRegisterDto users);
    public Task<AspNetUsersResponseDTO> GetUserInfo();
}