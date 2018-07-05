using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using System.Web;

namespace Sebrae.Academico.InfraEstrutura.Core.Nhibernate
{
    public sealed class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory { get; set; }

        public static ISession GetCurrentSession()
        {
            if (_sessionFactory == null)
            {
                if (HttpContext.Current == null)
                {
                    _sessionFactory = CreateSessionFactory<ThreadStaticSessionContext>();
                }
                else
                {
                    _sessionFactory = CreateSessionFactory<WebSessionContext>();
                }
            }

            if (CurrentSessionContext.HasBind(_sessionFactory))
                return _sessionFactory.GetCurrentSession();

            ISession session = _sessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);

            return session;
        }

        private static ISessionFactory CreateSessionFactory<T>() where T : ICurrentSessionContext
        {

            try
            {

                IPersistenceConfigurer persistenceConfigurer = null;

                string cnx = System.Configuration.ConfigurationManager.ConnectionStrings["cnxSebraeAcademico"].ConnectionString;

                persistenceConfigurer = MsSqlConfiguration
                        .MsSql2008
                        .IsolationLevel(System.Data.IsolationLevel.ReadCommitted)
                    //.UseOuterJoin()
#if DEBUG
                        .ShowSql()
                        .UseOuterJoin()
                        .FormatSql()
#endif
.ConnectionString(cnx);
                //.AdoNetBatchSize(30);

                FluentConfiguration nhbConfig = Fluently.Configure()
                   .CurrentSessionContext<T>()//.CurrentSessionContext("web") //("ManagedWebSessionContext")
                   .Database(persistenceConfigurer)
                   .Cache(it => it.Not.UseSecondLevelCache())
                   .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Sebrae.Academico.BM")));

                _sessionFactory = nhbConfig.BuildSessionFactory();
            }
            catch (FluentConfigurationException ex)
            {
                throw ex;
            }

            return _sessionFactory;
        }

    }
}
