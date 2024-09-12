using PracaInzynierska_RentIt.Server.Models.Application;

namespace PracaInzynierska_RentIt.Server.Models.Localization;

public class LocalizationEntity : ApplicationEntity
{
    public virtual String Name { get; set; }
    public virtual String Province { get; set; }
}