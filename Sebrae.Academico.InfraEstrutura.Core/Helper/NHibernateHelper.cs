using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public sealed class NHibernateHelper
    {
        public static ISessionFactory _sessionFactory = null;

        /// <summary>
        /// Construtor Privado.
        /// </summary>  
        private NHibernateHelper()
        {

        }

        public static ISessionFactory CriarSessionFactory()
        {
            SqlConnectionStringBuilder conexao = null;

            try
            {
                if (_sessionFactory != null) return _sessionFactory;

                //Obtém a string de conexão descriptografada
                string connectionStringDecriptografada = null;

                try
                {
                    //TODO: Analisar esta questão
                    //connectionStringDecriptografada = Classe.DecriptografarStringConexao(ConfigurationManager.ConnectionStrings["stringConexaoMySQL"].ToString());
                    connectionStringDecriptografada = ConfigurationManager.ConnectionStrings["cnxSebraeAcademico"].ConnectionString;
                    conexao = new SqlConnectionStringBuilder(connectionStringDecriptografada);

                    if (conexao == null)
                    {
                        throw new ApplicationException("Verifique a string de conexao com o banco de dados. Não foi possivel localizar o banco de dados do site.");
                    }
                }
                catch 
                {
                    //TODO: URGENTE: GERAR LOG DE ERROS PARA EU TOMAR UMA DECISAO
                    //TODO: LOGAR E MANDAR E-MAIL
                    //Criar um configurationException
                    throw new ApplicationException("Ocorreu um Problema no acesso ao banco de Dados. Verifique a string de Conexão");
                }

                IPersistenceConfigurer persistenceConfigurer = null;

                //Banco MYSQL

                string usuario, senha, database, servidor;
                usuario = conexao.UserID;
                senha = conexao.Password;
                database = conexao.InitialCatalog;
                servidor = conexao.DataSource;

                persistenceConfigurer = MsSqlConfiguration
                     .MsSql2008
                     .IsolationLevel(System.Data.IsolationLevel.ReadCommitted)
                    //.DefaultSchema("trilha")
                   .UseOuterJoin()
                   


//#if DEBUG
.ShowSql()
.FormatSql()
//#endif

                  .ConnectionString(c => c.Server(servidor).Username(usuario).Password(senha).Database(database))
                  .UseReflectionOptimizer()
                  .AdoNetBatchSize(250); 

                var configFluent = FluentNHibernate.Cfg.Fluently.Configure()

                    //ManagedWebSessionContext
                      .CurrentSessionContext<WebSessionContext>()

                 .Database(persistenceConfigurer)

                .Mappings(m =>
                {
                    //, DynamicUpdate.AlwaysTrue(), DynamicInsert.AlwaysTrue()
                    m.FluentMappings.Conventions.Add(DefaultLazy.Always());
                    m.FluentMappings.AddFromAssembly(Assembly.Load("Sebrae.Academico.BM"));
                }); //.ProxyFactoryFactory<NHibernate.ByteCode.Castle.ProxyFactoryFactory>();
                               

                _sessionFactory = configFluent.BuildSessionFactory();

                
            }
            catch (ApplicationException ex)
            {
                //   cUtil.WriteLog(ex, ex.Message);
                throw ex;
            }
            catch (FluentConfigurationException ex)
            {
                throw ex;
                // cUtil.WriteLog(ex, ex.Message, "Verificar os Mapeamentos. Pois é possível que algum mapeamento esteja errado.");
                //  throw new AcessoAoBancoDeDadosException("Ocorreu um Erro no Acesso ao Sistema.");
            }
            //catch (MySqlException ex)
            //{
            //    cUtil.WriteLog(ex, ex.Message, "Pode ser erro de memória. Verificar.");
            //    throw new AcessoAoBancoDeDadosException("Ocorreu um Erro no Acesso ao Sistema.");
            //}
            catch (System.Exception ex)
            {
                //cUtil.WriteLog(ex, ex.Message);
                //   throw new AcessoAoBancoDeDadosException("Ocorreu um Erro no Acesso ao Sistema.");
            }

            return _sessionFactory;
        }
    }
}

