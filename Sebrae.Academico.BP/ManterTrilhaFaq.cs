using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Sebrae.Academico.BP
{
    public class ManterTrilhaFaq : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMTrilhaFaq trilha = null;
        private BMAssuntoTrilhaFaq assunto = null;

        #endregion

        public ManterTrilhaFaq()
        {
            trilha = new BMTrilhaFaq();
            assunto = new BMAssuntoTrilhaFaq();
        }

        public IQueryable<TrilhaFaq> ObterTodosIQueryable()
        {
            return trilha.ObterTodosIQueryable();
        }

        public IQueryable<AssuntoTrilhaFaq> ObterTodosAssuntoIQueryable()
        {
            return assunto.ObterTodosIQueryable();
        }

        public IList<TrilhaFaq> ObterTodos()
        {
            return trilha.ObterTodos().ToList();
        }

        public IList<AssuntoTrilhaFaq> ObterTodosAssunto()
        {
            return assunto.ObterTodos();
        }

        public TrilhaFaq ObterPorId(int id)
        {
            return trilha.ObterPorId(id);
        }

        public AssuntoTrilhaFaq ObterAssuntoPorId(int id)
        {
            return assunto.ObterPorId(id);
        }

        public void Salvar(TrilhaFaq model)
        {
            trilha.Salvar(model);
        }

        public void SalvarAssunto(AssuntoTrilhaFaq model)
        {
            assunto.Salvar(model);
        }

        public void Excluir(TrilhaFaq model)
        {
            trilha.Excluir(model);
        }

        public void ExcluirAssunto(AssuntoTrilhaFaq model)
        {
            assunto.Excluir(model);
        }

        public IList<TrilhaFaq> ObterPorFiltro(TrilhaFaq model)
        {
            return trilha.ObterPorFiltro(model);
        }
    }
}
