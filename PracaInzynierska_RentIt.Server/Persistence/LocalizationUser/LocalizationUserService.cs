using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.LocalizationUser;

namespace PracaInzynierska_RentIt.Server.Persistence.LocalizationUser;

public class LocalizationUserService : ILocalizationUserService
{
    public LocalizationUserRepository Repository { get; }
    public LocalizationUserService(LocalizationUserRepository repository)
    {
        this.Repository = repository;
    }
}