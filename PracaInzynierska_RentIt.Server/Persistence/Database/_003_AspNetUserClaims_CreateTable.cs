using FluentMigrator;

namespace RentIt_PracaInzynierska.Server.Persistence.AspNetUsersEntity.Database;
[Migration(003)]
public class _003_AspNetUserClaims_CreateTable : Migration
{
    public override void Up()
    {
        Create.Table("AspNetUserClaims")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("UserId").AsString(128).NotNullable()
            .WithColumn("ClaimType").AsString(256).NotNullable()
            .WithColumn("ClaimValue").AsString(256).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("AspNetUserClaims");
    }
}