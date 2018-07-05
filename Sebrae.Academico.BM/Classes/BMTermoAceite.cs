using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTermoAceite : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<TermoAceite> repositorio;

        public BMTermoAceite()
        {
            repositorio = new RepositorioBase<TermoAceite>();
        }

        public TermoAceite ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(int pId)
        {
            TermoAceite termo = repositorio.ObterPorID(pId);
            repositorio.Excluir(termo);
        }

        public void ExcluirTermoAceiteCategoriaConteudo(int pId)
        {
            RepositorioBase<TermoAceiteCategoriaConteudo> rep = new RepositorioBase<TermoAceiteCategoriaConteudo>();
            TermoAceiteCategoriaConteudo termoCategoria = rep.ObterPorID(pId);
            rep.Excluir(termoCategoria);
        }

        public DTOTermoAceite ObterPorSolucacaoEducacional(int idSe)
        {
            var se = new BMSolucaoEducacional().ObterTodosPorGestor().FirstOrDefault(s => s.ID == idSe);

            var cat = se != null ? se.CategoriaConteudo : null; 

            var termo = cat != null ? cat.TermoAceiteCategoriaCounteudo != null
                ? se.CategoriaConteudo.TermoAceiteCategoriaCounteudo.TermoAceite
                : null : null;

            return termo != null ? new DTOTermoAceite(termo, se.ID, cat.ID) : null;
        }

        public DTOTermoAceite ObterPorCategoriaConteudo(int IdCategoriaConteudo)
        {
            ProcTermoAceite procTermoAceite = new ProcTermoAceite();

            return procTermoAceite.ObterTermoAdesao().Where(d => d.IdCategoriaConteudo == IdCategoriaConteudo).FirstOrDefault();
        }

        public IQueryable<TermoAceite> ObterListaPorCategoriaConteudo(int idCategoriaConteudo, int idTermoAceite)
        {
            var termos = repositorio.ObterTodosIQueryable();

            return
                termos.Where(
                    t =>
                        (idTermoAceite == 0 || t.ID == idTermoAceite) ||
                        t.ListaCategoriaConteudo.Any(c => c.CategoriaConteudo.ID == idCategoriaConteudo));
        }

        public IList<TermoAceite> ObterTodos()
        {
            return repositorio.ObterTodos().ToList();
        }

        public void SalvarTermoAceiteCategoriaConteudo(TermoAceiteCategoriaConteudo template)
        {
            new RepositorioBase<TermoAceiteCategoriaConteudo>().Salvar(template);
        }

        public void Salvar(TermoAceite template)
        {
            repositorio.Salvar(template);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IEnumerable<TermoAceite> ObterPorNome(string nome, bool parcial, bool caseSensitive)
        {
            nome = nome.ToLower();

            return
                repositorio.ObterTodos()
                    .Where(x => caseSensitive ? x.Nome.Contains(nome) : x.Nome.ToLower().Contains(nome));
        }
    }
}
