using Microsoft.AspNetCore.Mvc;

namespace PracaInzynierska_RentIt.Server.Models.Application;
[Route("api/[controller]")]
[ApiController]
public abstract class ApplicationController<T,TS, TR, TDto> : ControllerBase
    where T : ApplicationEntity
    where TS : IApplicationService<T,TR, TDto> where TR : IApplicationRepository<T ,TDto>
{
    public virtual TS service { get; }

    public ApplicationController(TS service)
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