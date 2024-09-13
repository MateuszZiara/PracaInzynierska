using PracaInzynierska_RentIt.Server.Models.Application;

namespace PracaInzynierska_RentIt.Server.Models.LocalizationUser;

public class LocalizationUserEntity : ApplicationEntity
{
    public virtual string UserId { get; set; }
    public virtual Guid LocalizationId { get; set; }
}