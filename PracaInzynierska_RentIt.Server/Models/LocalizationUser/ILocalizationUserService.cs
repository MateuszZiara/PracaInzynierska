using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Persistence.LocalizationUser;

namespace PracaInzynierska_RentIt.Server.Models.LocalizationUser;

public interface ILocalizationUserService : IApplicationService<LocalizationUserEntity, LocalizationUserRepository, LocalizationUserEntity>
{
    
}