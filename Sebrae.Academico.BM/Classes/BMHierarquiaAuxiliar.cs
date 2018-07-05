using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMHierarquiaAuxiliar : BusinessManagerBase, IDisposable
    {

        private RepositorioBase<HierarquiaAuxiliar> repositorio;


        public BMHierarquiaAuxiliar()
        {
            repositorio = new RepositorioBase<HierarquiaAuxiliar>();
        }

        public HierarquiaAuxiliar ObterPorId(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void ValidarHierarquiaAuxInformada(HierarquiaAuxiliar pHierarquiaAuxiliar)
        {
            this.ValidarInstancia(pHierarquiaAuxiliar);

            if (pHierarquiaAuxiliar.CodUnidade == "") throw new AcademicoException("Deve ser especifica.");

            if (pHierarquiaAuxiliar.Usuario == null) throw new AcademicoException("Acessor. Campo Obrigatório.");                        
        }

        public void Salvar(HierarquiaAuxiliar pOferta)
        {
            ValidarHierarquiaAuxInformada(pOferta);
            repositorio.Salvar(pOferta);
        }

        public IList<HierarquiaAuxiliar> ObterPorNomeDeUsuario(string nome)
        {
            var query = repositorio.session.Query<HierarquiaAuxiliar>();

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(x => x.Usuario.Nome.ToUpper().Contains(nome.ToUpper())).AsQueryable();            

            return query.OrderBy(x => x.CodUnidade).ThenBy(x => x.Usuario.Nome).ToList();
        }

        public IQueryable<HierarquiaAuxiliar> ObterTodos()
        {
            return repositorio.session.Query<HierarquiaAuxiliar>();
        }

        public void ExcluirHierarquiaAuxiliar(HierarquiaAuxiliar pHierarquiaAuxiliar)
        {            
            repositorio.Excluir(pHierarquiaAuxiliar);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
