using FluentNHibernate.Mapping;

namespace PracaInzynierska_RentIt.Server.Models.Localization;

public class LocalizationEntityMapping : ClassMap<LocalizationEntity>
{
    readonly string _tablename = nameof(LocalizationEntity);

    public LocalizationEntityMapping()
    {
        Id(x => x.Id);
        Map(x => x.Name);
        Map(x => x.Province);
        Map(x => x.ModifiedBy);
        Map(x => x.ModifiedTime);
        Map(x => x.CreateBy);
        Map(x => x.CreateTime);
    }
}