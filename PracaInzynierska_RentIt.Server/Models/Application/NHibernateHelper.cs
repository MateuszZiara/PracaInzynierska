using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public class NHibernateHelper
{
    private static ISessionFactory _sessionFactory;

    public static NHibernate.ISession OpenSession()
    {
        return SessionFactory.OpenSession();
    }

    private static ISessionFactory SessionFactory
    {
        get
        {
            if (_sessionFactory == null)
            {
                _sessionFactory = Fluently.Configure()
                    .Database(
                        MsSqlConfiguration.MsSql2012.ConnectionString(
                            "Server=localhost\\SQLEXPRESS;Database=RentIt;Integrated Security=SSPI;Application Name=RentIt;TrustServerCertificate=true;")
                    ) 
                    .Mappings(m =>
                       m.AutoMappings.Add(AutoMap.AssemblyOf<AspNetUsers>()))
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .BuildSessionFactory();
            }
            return _sessionFactory;
        }
    }
}