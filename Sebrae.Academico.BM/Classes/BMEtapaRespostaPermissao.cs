using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;



namespace Sebrae.Academico.BM.Classes
{
    public class BMEtapaRespostaPermissao : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<EtapaRespostaPermissao> repositorio;
        public ISession session = NHibernateSessionManager.GetCurrentSession();

        public BMEtapaRespostaPermissao()
        {
            repositorio = new RepositorioBase<EtapaRespostaPermissao>();
        }

        public IList<EtapaRespostaPermissao> ObterTodos()
        {
            var query = repositorio.session.Query<EtapaRespostaPermissao>();
            return query.ToList<EtapaRespostaPermissao>();
        }

        public EtapaRespostaPermissao ObterPorId(int pId)
        {
            var query = repositorio.session.Query<EtapaRespostaPermissao>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public void Salvar(EtapaRespostaPermissao permissao)
        {
            repositorio.Salvar(permissao);
        }

        public void Salvar(IList<EtapaRespostaPermissao> permissao)
        {
            repositorio.Salvar(permissao);
        }

        public void Excluir(EtapaRespostaPermissao permissao)
        {
            repositorio.Excluir(permissao);
        }

        public void ExcluirTodosFkEtapaPermissaoNucleo(int fk)
        {           
            var rowsAffected = session.CreateSQLQuery(@"
            DELETE from TB_EtapaRespostaPermissao where ID_EtapaPermissaoNucleo = :val; 
            select @@ROWCOUNT NumberOfRows;")
             .AddScalar("NumberOfRows", NHibernateUtil.Int32)
             .SetParameter("val", fk)
             .UniqueResult();
        }

        public void Dispose()
        {
            repositorio.Dispose();
        }
    }
}
