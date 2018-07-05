using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMAreaTematica : BusinessManagerBase, IDisposable
    {
        #region Atributos
        private RepositorioBase<AreaTematica> repositorio;
        #endregion

        #region "Construtor"

        public BMAreaTematica(){
            repositorio = new RepositorioBase<AreaTematica>();
        }

        #endregion

        public IList<AreaTematica> ObterTodos(){
            var query = repositorio.session.Query<AreaTematica>();
            return query.ToList();
        }

        public AreaTematica ObterPorId(int pId){
            var query = repositorio.session.Query<AreaTematica>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<AreaTematica> ObterPorFiltro(AreaTematica obj){
            var query = repositorio.session.Query<AreaTematica>();
            if (obj == null) return query.ToList();
            if (!string.IsNullOrEmpty(obj.Nome)){
                query = query.Where(p => p.Nome.ToLower().Contains(obj.Nome.ToLower()));
            }
            return query.ToList();
        }

        public void Salvar(AreaTematica model){
            repositorio.Salvar(model);
        }

        public void Excluir(AreaTematica model){
            repositorio.Excluir(model);
        }
        public void Dispose(){
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
