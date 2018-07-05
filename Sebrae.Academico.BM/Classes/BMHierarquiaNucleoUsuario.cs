using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using NHibernate;


namespace Sebrae.Academico.BM.Classes
{

    public class BMHierarquiaNucleoUsuario : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<HierarquiaNucleoUsuario> repositorio;
        public ISession session = NHibernateSessionManager.GetCurrentSession();

        public BMHierarquiaNucleoUsuario()
        {
            repositorio = new RepositorioBase<HierarquiaNucleoUsuario>();
        }

        public HierarquiaNucleoUsuario ObterPorId(int idHierarquiaNucleoUsuario)
        {
            return repositorio.session.Query<HierarquiaNucleoUsuario>().FirstOrDefault(x => x.ID == idHierarquiaNucleoUsuario);
        }

        public HierarquiaNucleoUsuario ObterPorUsuarioEHieraquiaNucleo(int idUsario, int idHierarquiaNucleo)
        {
            return repositorio.session.Query<HierarquiaNucleoUsuario>().FirstOrDefault(x => x.Usuario.ID == idUsario && x.HierarquiaNucleo.ID == idHierarquiaNucleo);
        }

        public IQueryable<HierarquiaNucleoUsuario> ObterTodos()
        {
            return repositorio.session.Query<HierarquiaNucleoUsuario>().AsQueryable();
        }

        public IQueryable<HierarquiaNucleoUsuario> ObterUsuariosNucleo(int idUsuario)
        {
            return repositorio.session.Query<HierarquiaNucleoUsuario>().AsQueryable().Where(x => x.Usuario.ID == idUsuario );
        }

        public void Salvar(HierarquiaNucleoUsuario hierarquiaNucleoUsuario)
        {
            repositorio.session.SaveOrUpdate(hierarquiaNucleoUsuario);
        }

        public void Remover(int idHierarquiaNucleoUsuario)
        {
            repositorio.Excluir(new HierarquiaNucleoUsuario { ID = idHierarquiaNucleoUsuario });
        }

        public void Excluir(int idHierarquiaNucleoUsuario)
        {
            var rowsAffected = session.CreateSQLQuery(@"
            DELETE from TB_HierarquiaNucleoUsuario where ID_HierarquiaNucleoUsuario = :val; 
            select @@ROWCOUNT NumberOfRows;")
             .AddScalar("NumberOfRows", NHibernateUtil.Int32)
             .SetParameter("val", idHierarquiaNucleoUsuario)
             .UniqueResult();
        }

        public void ExcluirTodosFkHieraquiaNucleoUsuario(int fk)
        {
            var rowsAffected = session.CreateSQLQuery(@"
            DELETE from TB_EtapaPermissaoNucleo where ID_HierarquiaNucleoUsuario = :val; 
            select @@ROWCOUNT NumberOfRows;")
             .AddScalar("NumberOfRows", NHibernateUtil.Int32)
             .SetParameter("val", fk)
             .UniqueResult();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
