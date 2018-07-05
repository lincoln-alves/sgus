using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Context;
using System.Web;
using System.Reflection;

namespace Sebrae.Academico.InfraEstrutura.Core.Nhibernate
{
    public class NHibernateSessionManager
    {
        private static ISessionFactory Factory { get; set; }

        private static ISessionFactory FactoryMdl { get; set; }

        private static ISessionFactory FactoryPortal { get; set; }

        private static ISessionFactory FactoryConheciGame { get; set; }

        public static string ConnectionString { get; set; }


        static NHibernateSessionManager()
        {
            ConnectionString = string.Empty;
        }


        private static ISessionFactory GetFactory<T>() where T : ICurrentSessionContext
        {
            string cnx = System.Configuration.ConfigurationManager.ConnectionStrings["cnxSebraeAcademico"].ConnectionString;
            
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                 #if DEBUG
                 .ShowSql()
                 #endif
                 .ConnectionString(cnx))
                 .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Sebrae.Academico.BM")))
                .CurrentSessionContext<T>().BuildSessionFactory();
        }

        private static ISessionFactory GetFactoryMdl<T>() where T : ICurrentSessionContext
        {
            string cnx = System.Configuration.ConfigurationManager.ConnectionStrings["cnxMoodle"].ConnectionString;
            return Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                #if DEBUG
                .ShowSql()
                #endif
                .ConnectionString(cnx))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Sebrae.Academico.BM")))
                .CurrentSessionContext<T>().BuildSessionFactory();
        }

        private static ISessionFactory GetFactoryConhecigame<T>() where T : ICurrentSessionContext
        {
            string cnx = System.Configuration.ConfigurationManager.ConnectionStrings["cnxConheciGame"].ConnectionString;
            return Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                #if DEBUG
                .ShowSql()
                #endif
                .ConnectionString(cnx))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Sebrae.Academico.BM")))
                .CurrentSessionContext<T>().BuildSessionFactory();
        }

        private static ISessionFactory GetFactoryPortal<T>() where T : ICurrentSessionContext
        {
            string cnx = System.Configuration.ConfigurationManager.ConnectionStrings["cnxPortal"].ConnectionString;
            return Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                #if DEBUG
                .ShowSql()
                #endif
                .ConnectionString(cnx))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Sebrae.Academico.BM")))
                .CurrentSessionContext<T>().BuildSessionFactory();
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        public static ISession GetCurrentSession()
        {
            if (Factory == null)
                Factory = HttpContext.Current != null ? GetFactory<WebSessionContext>() : GetFactory<ThreadStaticSessionContext>();

            if (CurrentSessionContext.HasBind(Factory))
                return Factory.GetCurrentSession();

            var session = Factory.OpenSession();
            CurrentSessionContext.Bind(session);

            return session;
        }


        /// <summary>
        /// Gets the current session just for Mysql to Moodle.
        /// </summary>
        public static ISession GetCurrentSessionMdl()
        {
            FactoryMdl = HttpContext.Current != null ? GetFactoryMdl<WebSessionContext>() : GetFactoryMdl<ThreadStaticSessionContext>();

            if (CurrentSessionContext.HasBind(FactoryMdl))
                return FactoryMdl.GetCurrentSession();

            var session = FactoryMdl.OpenSession();
            CurrentSessionContext.Bind(session);

            return session;
        }

        /// <summary>
        /// Gets the current session just for Mysql to ConheciGame.
        /// </summary>
        public static ISession GetCurrentSessionConheciGame()
        {
            FactoryConheciGame = HttpContext.Current != null ? GetFactoryConhecigame<WebSessionContext>() : GetFactoryConhecigame<ThreadStaticSessionContext>();

            if (CurrentSessionContext.HasBind(FactoryConheciGame))
                return FactoryConheciGame.GetCurrentSession();

            var session = FactoryConheciGame.OpenSession();
            CurrentSessionContext.Bind(session);

            return session;
        }


        /// <summary>
        /// Gets the current session just for Mysql to Portal.
        /// </summary>
        public static ISession GetCurrentSessionPortal()
        {
            FactoryPortal = HttpContext.Current != null ? GetFactoryPortal<WebSessionContext>() : GetFactoryPortal<ThreadStaticSessionContext>();

            if (CurrentSessionContext.HasBind(FactoryPortal))
                return FactoryPortal.GetCurrentSession();

            var session = FactoryPortal.OpenSession();
            CurrentSessionContext.Bind(session);

            return session;
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public static void CloseSession()
        {
            if (Factory != null && CurrentSessionContext.HasBind(Factory))
            {
                var session = CurrentSessionContext.Unbind(Factory);
                session.Close();
            }
        }


        /// <summary>
        /// Commits the session.
        /// </summary>
        /// <param name="session">The session.</param>
        public static void CommitSession(ISession session)
        {
            try
            {
                session.Transaction.Commit();
            }
            catch (Exception)
            {
                session.Transaction.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Creates the database from mapping in this assembly
        /// </summary>
        public static void CreateSchemaFromMappings()
        {
            var config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                .ShowSql()
                .ConnectionString(c => c.Is(ConnectionString)))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateSessionManager>());

            new SchemaExport(config.BuildConfiguration()).Create(false, true);
        }
    }
}
