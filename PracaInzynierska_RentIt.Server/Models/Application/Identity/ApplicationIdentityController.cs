using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;

namespace PracaInzynierska_RentIt.Server.Models.Application;
[Route("api/[controller]")]
[ApiController]
public abstract class ApplicationIdentityController<T,TS, TR, TDto> : ControllerBase 
    where T : IdentityUser
    where TS : IApplicationIdentityService<T,TR, TDto> where TR : IApplicationIdentityRepository<T ,TDto>
{
    public virtual TS service { get; }

    public ApplicationIdentityController(TS service)
    {
        this.service = service;
    }
    [HttpGet("GetAll")]
    public List<TDto> GetAll() => service.GetAll();
    [HttpGet("GetById")]
    public ActionResult<TDto> GetById(Guid id) => service.GetById(id);
    [HttpPost("Create")]
    public ActionResult<TDto> Create(T t) => service.Create(t);
    [HttpPost("Delete")]
    public  bool Delete(Guid id)=> service.Delete(id);
}