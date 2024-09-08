using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Persistence.Localization;

namespace PracaInzynierska_RentIt.Server.Models.Localization;

public interface ILocalizationService : IApplicationService<LocalizationEntity, LocalizationEntity, LocalizationRepository>
{
    
}