using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska_RentIt.Server.Models.Application.DBContext;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Models.Localization;
using PracaInzynierska_RentIt.Server.Models.LocalizationUser;
using PracaInzynierska_RentIt.Server.Persistence.AspNetUsersEntity;
using PracaInzynierska_RentIt.Server.Persistence.Localization;
using PracaInzynierska_RentIt.Server.Persistence.LocalizationUser;
using RentIt_PracaInzynierska.Server.Models.IdentityLogin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AspNetUsersRepository>();
builder.Services.AddScoped<AspNetUsersService>();
builder.Services.AddScoped<IAspNetUsersService, AspNetUsersService>();

builder.Services.AddScoped<LocalizationRepository>();
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();

builder.Services.AddScoped<LocalizationUserRepository>();
builder.Services.AddScoped<LocalizationUserService>();
builder.Services.AddScoped<ILocalizationUserService, LocalizationUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("https://localhost:5173");
            builder.AllowCredentials();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });
});
builder.Services.AddFluentMigratorCore() 
    .ConfigureRunner(c =>
    {
        c.AddSqlServer2016()
            .WithGlobalConnectionString("Server=localhost\\SQLEXPRESS;Database=RentIt;Integrated Security=SSPI;Application Name=RentIt; TrustServerCertificate=true;")
            .ScanIn(Assembly.GetExecutingAssembly()).For.All();
    })
    .AddLogging(config => config.AddFluentMigratorConsole());
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        "Server=localhost\\SQLEXPRESS;Database=RentIt;Integrated Security=SSPI;Application Name=RentIt; TrustServerCertificate=true;"));
builder.Services.AddIdentityApiEndpoints<AspNetUsers>()
    .AddEntityFrameworkStores<DataContext>();
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapIdentityApi<AspNetUsers>();
app.MapIdentityApiCustom<AspNetUsers>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using var scope = app.Services.CreateScope();
var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();
if (migrator != null)
{
    try
    {
        migrator.ListMigrations();
        migrator.MigrateUp();
    }
    catch
    {
        Console.Error.WriteLine("There are no migrations!");
    }
}else
{
    throw new Exception("Migration fault");
}
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");
app.Run();
