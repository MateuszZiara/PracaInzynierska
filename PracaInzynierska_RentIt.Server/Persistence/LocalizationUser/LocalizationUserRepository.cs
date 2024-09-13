using PracaInzynierska_RentIt.Server.Models.LocalizationUser;

namespace PracaInzynierska_RentIt.Server.Persistence.LocalizationUser;

public class LocalizationUserRepository : ILocalizationUserRepository
{
    public LocalizationUserEntity ConvertToDto(LocalizationUserEntity? entity)
    {
        if (entity != null) return entity;
        throw new NullReferenceException();
    }
}