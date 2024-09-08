using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PracaInzynierska_RentIt.Server.Models.Application;
[Route("api/[controller]")]
[ApiController]
public class ApplicationIdentityController<T,TS, TR> : ControllerBase 
    where T : IdentityUser
    where TS : IApplicationIdentityService<T,TR> where TR : IApplicationIdentityRepository<T>
{
    public virtual TS service { get; }

    public ApplicationIdentityController(TS service)
    {
        this.service = service;
    }
    [HttpGet("GetAll")]
    public List<T> GetAll() => service.GetAll();
    [HttpGet("GetById")]
    public ActionResult<T> GetById(Guid id) => service.GetById(id);
    [HttpGet("Create")]
    public ActionResult<T> Create(T t) => service.Create(t);
    [HttpGet("Create")]
    public  bool Delete(Guid id)=> service.Delete(id);
}