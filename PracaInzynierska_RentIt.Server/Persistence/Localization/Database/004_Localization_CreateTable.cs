using FluentMigrator;
using PracaInzynierska_RentIt.Server.Models.Localization;
using Newtonsoft.Json.Linq;
namespace PracaInzynierska_RentIt.Server.Persistence.Localization.Database;
[Migration(004)]
public class _004_Localization_CreateTable : Migration
{
    public override void Up()
    {
        Create.Table(nameof(LocalizationEntity))
            .WithColumn(nameof(LocalizationEntity.Id)).AsGuid().PrimaryKey()
            .WithColumn(nameof(LocalizationEntity.CreateBy)).AsString().Nullable()
            .WithColumn(nameof(LocalizationEntity.CreateTime)).AsDateTime().Nullable()
            .WithColumn(nameof(LocalizationEntity.ModifiedBy)).AsString().Nullable()
            .WithColumn(nameof(LocalizationEntity.ModifiedTime)).AsDateTime().Nullable()
            .WithColumn(nameof(LocalizationEntity.Name)).AsString();
            
        InsertDataFromApi().GetAwaiter().GetResult();
    }

    public override void Down()
    {
        Delete.Table(nameof(LocalizationEntity));
    }

    private async Task InsertDataFromApi()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync("https://secure.geonames.org/searchJSON?username=rentit&country=pl&featureClass=P&maxRows=1000");
        var data = JObject.Parse(response)["geonames"];

        foreach (var location in data)
        {
            var name = location["name"].ToString();
            var id = Guid.NewGuid();

            Insert.IntoTable(nameof(LocalizationEntity)).Row(new
            {
                Id = id,
                Name = name
            });
        }
    }
}