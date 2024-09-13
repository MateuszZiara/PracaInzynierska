using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.LocalizationUser;
using PracaInzynierska_RentIt.Server.Models.LocalizationUser.Dto;
using PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Persistence.LocalizationUser;

namespace PracaInzynierska_RentIt.Server.Controllers.LocalizationUser;
[Route("api/[controller]")]
[ApiController]
public class LocalizationUserController : ApplicationController<LocalizationUserEntity,LocalizationUserService, LocalizationUserRepository, LocalizationUserEntity>
{
    private AspNetUsersRepository _repository;
    public LocalizationUserController(LocalizationUserService service, AspNetUsersRepository repository) : base(service)
    {
        _repository = repository;
    }

    [HttpPost("CreateMany")]
    public IActionResult CreateMany([FromBody] LocalizationRequest request)
    {
        int numberOfSuccess = 0;
        using (var session = NHibernateHelper.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                for (int i = 0; i < request.LocalizationId.Length; ++i)
                {
                    try
                    {
                        var user = _repository.GetUserInfo();
                        LocalizationUserEntity localizationUserEntity = new LocalizationUserEntity();
                        localizationUserEntity.LocalizationId = new Guid(request.LocalizationId[i]);
                        localizationUserEntity.UserId = user.Id;
                        session.Save(localizationUserEntity);
                        numberOfSuccess++;
                    }
                    catch (Exception ex)
                    {
                        // Optionally log the error
                        // Console.WriteLine(ex.Message);
                    }
                }
                transaction.Commit();
            }
        }

        return Ok(numberOfSuccess);
    }

}