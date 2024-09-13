using FluentNHibernate.Mapping;
using PracaInzynierska_RentIt.Server.Models.Localization;

namespace PracaInzynierska_RentIt.Server.Models.LocalizationUser;

public class LocalizationUserMapping : ClassMap<LocalizationUserEntity>
{
    readonly string _tablename = nameof(LocalizationUserEntity);
    public LocalizationUserMapping()
    {
        Id(x => x.Id);
        Map(x => x.UserId);
        Map(x => x.LocalizationId);
        Map(x => x.ModifiedBy);
        Map(x => x.CreateBy);
        Map(x => x.CreateTime);
        Map(x => x.ModifiedTime);
        Table(_tablename);
    }
}