using System;
using NHibernate;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Web;

namespace Sebrae.Academico.InfraEstrutura.Core
{
    public class SessionManager
    {
        public ISession session;
        private const string Sessionkey = "NHIBERNATE.SESSION";
        [ThreadStatic]
        private ISession _session; //this session is not used in web
        //private static ISession openSession; // Para SQLite
        //  protected Configuration _configuration;
        protected ISessionFactory _sessionFactory;

        //public static bool isSqlLite;
        //public static bool isOracle;
        //public static bool ConexaoSqlLite;
        //public static bool ConexaoOracle;
        //public static int nContaVezes = 0;

        #region Constructor

        #region Singleton

        public SessionManager Instance
        {
            get
            {
                Singleton sg = new Singleton();
                return sg.Instance;
            }
        }
        private class Singleton
        {
            static Singleton() { }
            internal SessionManager Instance { get { return getInstance(); } }

            private SessionManager getInstance() 
            {
                if (HttpContext.Current.Application["secao"] == null) 
                {
                    HttpContext.Current.Application.Add("secao", new SessionManager());
                }

              
                return (SessionManager)HttpContext.Current.Application["secao"];

            }

        }

        #endregion

        private static string gerarLog(System.Exception e)
        {
            string content = string.Empty;
            if (e.InnerException != null)
                content += System.Environment.NewLine + gerarLog(e.InnerException) + System.Environment.NewLine;
            else
                content = System.Environment.NewLine + e.ToString() + System.Environment.NewLine;

            return content;
        }

        #endregion

        //#region Configuration Serialization
        ////specify a full path if required
        //private const string ConfigurationFilePath = "Configuration.save";
        //private static void SaveConfiguration(Configuration configuration)
        //{
        //    IFormatter serializer = new BinaryFormatter();

        //    using (Stream stream = File.OpenWrite(ConfigurationFilePath))
        //    {
        //        try
        //        {
        //            serializer.Serialize(stream, configuration);
        //        }
        //        catch (UnauthorizedAccessException)
        //        {
        //            Console.WriteLine("No write access to " + ConfigurationFilePath);
        //        }
        //    }
        //}

        //#endregion

        #region NHibernate Setup
        /// <summary>
        /// Gets the <see cref="NHibernate.Cfg.Configuration"/>.
        /// </summary>
        // internal Configuration Configuration { get { return _configuration; } }

        /// <summary>
        /// Gets the <see cref="NHibernate.ISessionFactory"/>
        /// </summary>
        internal ISessionFactory SessionFactory { get { return _sessionFactory; } }

        /// <summary>
        /// Closes the session factory.
        /// </summary>
        public void Close()
        {
            //if (isSqlLite)
            //{
            //    openSession.Close();
            //    nContaVezes = 0;
            //}
            //else
            SessionFactory.Close();
        }

        internal static bool IsWeb { get { return (HttpContext.Current != null); } }
        #endregion


        #region NHibernate Sessions

        /// <summary>
        /// Explicitly open a session. If you have an open session, close it first.
        /// </summary>
        /// <returns>The <see cref="NHibernate.ISession"/></returns>
        public ISession OpenSession()
        {

            //if (session != null && session.IsOpen)
            //    _session = session;
            //else
                //ISession session = SessionFactory.OpenSession();
                session = SessionFactory.OpenSession();

            //if (IsWeb && HttpContext.Current.Items[Sessionkey] == null)
            //    HttpContext.Current.Items.Add(Sessionkey, session);
            //else
            //    _session = session;

            return session;
        }

        /// <summary>
        /// Gets the current <see cref="NHibernate.ISession"/>. 
        /// Although this is a singleton, this is specific to the thread/ asp session. 
        /// If you want to handle multiple sessions, use <see cref="OpenSession"/> directly. 
        /// If a session it not open, a new open session is created and returned.
        /// </summary>
        /// <value>The <see cref="NHibernate.ISession"/></value>
        public ISession Session
        {
            get
            {

                //if (isSqlLite)
                //{
                //    if (SessionFactory == null)
                //    {
                //        //_configuration = ObtemConfiguracao();
                //        _sessionFactory = NHibernateHelper.CriarSessionFactory(); // Configuration.BuildSessionFactory();
                //    }

                //    if (nContaVezes == 0)
                //    {
                //        openSession = SessionFactory.OpenSession();
                //        IDbConnection connection = openSession.Connection;
                //        //  new SchemaExport(cfg).Execute(false, true, false, true, connection, null);
                //        new SchemaExport(Configuration).Execute(false, true, false, connection, null);
                //        nContaVezes += 1;
                //        return openSession;
                //    }
                //    else
                //    {
                //        return openSession;
                //    }
                //}

                //use threadStatic or asp session.
               // ISession session = IsWeb ? HttpContext.Current.Items[Sessionkey] as ISession : _session;
                //if using CurrentSessionContext, SessionFactory.GetCurrentSession() can be used

                //if it's an open session, that's all
                if (session != null && session.IsOpen)
                    return session;

                //if not open, open a new session
                return OpenSession();
            }
        }
        #endregion

    }
}



