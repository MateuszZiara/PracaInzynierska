using FluentMigrator;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
namespace RentIt_PracaInzynierska.Server.Persistence.AspNetUsersEntity.Database;
[Migration(001)]
public class _001_AspNetUsers_CreateTable : Migration
{
    public override void Up()
    {
        Create.Table("AspNetUsers")
            .WithColumn("Id").AsString().NotNullable().PrimaryKey()
            .WithColumn("UserName").AsString().NotNullable()
            .WithColumn("NormalizedUserName").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("NormalizedEmail").AsString().NotNullable()
            .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
            .WithColumn("PasswordHash").AsString().NotNullable()
            .WithColumn("SecurityStamp").AsString().Nullable()
            .WithColumn("ConcurrencyStamp").AsString().Nullable()
            .WithColumn("PhoneNumber").AsString().Nullable().Nullable()
            .WithColumn("PhoneNumberConfirmed").AsBoolean().Nullable()
            .WithColumn("TwoFactorEnabled").AsBoolean().Nullable()
            .WithColumn("LockoutEnd").AsDateTimeOffset().Nullable()
            .WithColumn("LockoutEnabled").AsBoolean().Nullable()
            .WithColumn("AccessFailedCount").AsInt32().Nullable()
            //Custom
            .WithColumn(nameof(AspNetUsers.FirstName)).AsString().Nullable()
            .WithColumn(nameof(AspNetUsers.LastName)).AsString().Nullable()
            .WithColumn(nameof(AspNetUsers.Name)).AsString().Nullable()
            .WithColumn(nameof(AspNetUsers.Provider)).AsString().Nullable()
            .WithColumn(nameof(AspNetUsers.Avatar)).AsBoolean().Nullable()
            .WithColumn(nameof(AspNetUsers.CreateBy)).AsString().Nullable()
            .WithColumn(nameof(AspNetUsers.CreateTime)).AsDateTime().Nullable()
            .WithColumn(nameof(AspNetUsers.ModifiedBy)).AsString().Nullable()
            .WithColumn(nameof(AspNetUsers.ModifiedTime)).AsDateTime().Nullable()
            .WithColumn(nameof(AspNetUsers.BirthDate)).AsDate().NotNullable()
            .WithColumn("Lokalizacja").AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(AspNetUsers.UserRank)).AsInt32().Nullable();
        
    }
    public override void Down()
    {
        if (Schema.Table("AspNetUsers").Exists())
        {
            Delete.Table("AspNetUsers");
        }
    }
}