
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PracaInzynierska_RentIt.Server.Models.Application.DBContext;

public class DataContext : IdentityDbContext<AspNetUsers>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options){}
}