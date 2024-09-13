using FluentMigrator;
using FluentMigrator.Expressions;
using PracaInzynierska_RentIt.Server.Models.Localization;
using Newtonsoft.Json.Linq;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.LocalizationUser;

namespace PracaInzynierska_RentIt.Server.Persistence.Database;
[Migration(005)]
public class _005_LocalizationUser_CreateTable : Migration
{
    public override void Up()
    {
        Create.Table(nameof(LocalizationUserEntity))
            .WithColumn(nameof(LocalizationUserEntity.Id)).AsGuid().PrimaryKey().NotNullable() 
            .WithColumn(nameof(LocalizationUserEntity.UserId)).AsString().NotNullable()
            .WithColumn(nameof(LocalizationUserEntity.LocalizationId)).AsGuid().NotNullable();

        Create.ForeignKey(nameof(LocalizationUserEntity) + nameof(AspNetUsers))
            .FromTable(nameof(LocalizationUserEntity)).ForeignColumn(nameof(LocalizationUserEntity.UserId))
            .ToTable(nameof(AspNetUsers)).PrimaryColumn(nameof(AspNetUsers.Id));
        Create.ForeignKey(nameof(LocalizationUserEntity) + nameof(LocalizationEntity))
            .FromTable(nameof(LocalizationUserEntity)).ForeignColumn(nameof(LocalizationUserEntity.LocalizationId))
            .ToTable(nameof(LocalizationEntity)).PrimaryColumn(nameof(LocalizationEntity.Id));
    }

    public override void Down()
    {
        Delete.Table(nameof(LocalizationUserEntity));
    }
}