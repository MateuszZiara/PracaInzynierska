using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;

namespace RentIt_PracaInzynierska.Server.Models.AspNetUsersEntity;
using FluentNHibernate.Mapping;
public class AspNetUsersMapping : ClassMap<AspNetUsers>
{
    readonly string _tablename = nameof(AspNetUsers);
    public AspNetUsersMapping()
    {
        //Custom
        Id(x => x.Id);
        Map(x => x.Provider);
        Map(x => x.FirstName);
        Map(x => x.LastName);
        Map(x => x.UserRank).CustomType<UserRank>();
        Map(x => x.Name);
        Map(x => x.ModifiedBy);
        Map(x => x.CreateBy);
        Map(x => x.CreateTime);
        Map(x => x.ModifiedTime);
        //Identity
        Map(x => x.UserName);
        Map(x => x.NormalizedUserName);
        Map(x => x.Email);
        Map(x => x.NormalizedEmail);
        Map(x => x.EmailConfirmed);
        Map(x => x.PasswordHash);
        Map(x => x.SecurityStamp);
        Map(x => x.ConcurrencyStamp);
        Map(x => x.PhoneNumber);
        Map(x => x.PhoneNumberConfirmed);
        Map(x => x.TwoFactorEnabled);
        Map(x => x.LockoutEnd);
        Map(x => x.LockoutEnabled);
        Map(x => x.AccessFailedCount);
        Table(_tablename);
    }
}