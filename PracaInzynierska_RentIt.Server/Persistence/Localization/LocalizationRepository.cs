using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;
using PracaInzynierska_RentIt.Server.Models.Localization;

namespace PracaInzynierska_RentIt.Server.Persistence.Localization;

public class LocalizationRepository : ILocalizationRepository
{
    
    public ActionResult<LocalizationEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public ActionResult<LocalizationEntity> Create(LocalizationEntity t)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool Edit(LocalizationEntity newEntity)
    {
        throw new NotImplementedException();
    }
    
}