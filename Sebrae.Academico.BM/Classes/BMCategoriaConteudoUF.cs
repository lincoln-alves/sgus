using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCategoriaConteudoUF : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<CategoriaConteudoUF> repositorio;

        public IList<CategoriaConteudoUF> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CategoriaConteudoUF>();
            return query.ToList<CategoriaConteudoUF>();
        }

        public CategoriaConteudoUF ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CategoriaConteudoUF>();
            return query.FirstOrDefault(x => x.ID == pId);
        }   

        public void Salvar(CategoriaConteudoUF model)
        {

            //Caso seja unico, descomentar a linha baixo e implementar
            //a verificacao por nome do programa.
            if (model.ID == 0)
            {
                if (this.ObterPorId(model.ID) != null)
                {
                    throw new AcademicoException("Já existe um registro.");
                }
            }

            repositorio.Salvar(model);

        }

        public void Excluir(CategoriaConteudoUF model)
        {
            repositorio.Excluir(model);
        }

        private void ValidarModuloInformada(CategoriaConteudoUF model)
        {
            //throw new NotImplementedException();
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
