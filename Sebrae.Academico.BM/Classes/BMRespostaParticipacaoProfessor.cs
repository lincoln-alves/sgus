using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMRespostaParticipacaoProfessor : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<RespostaParticipacaoProfessor> repositorio;

        public BMRespostaParticipacaoProfessor()
        {
            repositorio = new RepositorioBase<RespostaParticipacaoProfessor>();
        }
        
        public void ValidarItemQuestionarioInformado(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            ValidarInstancia(pRespostaParticipacaoProfessor);
        }

        public void Salvar(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            ValidarItemQuestionarioInformado(pRespostaParticipacaoProfessor);
            repositorio.Salvar(pRespostaParticipacaoProfessor);
        }
        public void Excluir(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            if (this.ValidarDependencias(pRespostaParticipacaoProfessor))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta informação.");

            repositorio.Excluir(pRespostaParticipacaoProfessor);
        }

        public IList<RespostaParticipacaoProfessor> ObterTodos()
        {
            return repositorio.ObterTodos().ToList<RespostaParticipacaoProfessor>();
        }

        public RespostaParticipacaoProfessor ObterPorID(int pId)
        {
            return repositorio.session.Query<RespostaParticipacaoProfessor>().FirstOrDefault(x => x.ID == pId);
        }

        public IList<RespostaParticipacaoProfessor> ObterPorFiltro(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            var query = repositorio.session.Query<RespostaParticipacaoProfessor>();

            if (pRespostaParticipacaoProfessor != null)
            {
                if (pRespostaParticipacaoProfessor.ItemQuestionarioParticipacao != null) {
                    query = query.Where(x => x.ItemQuestionarioParticipacao.ID == pRespostaParticipacaoProfessor.ItemQuestionarioParticipacao.ID);
                }
                if (pRespostaParticipacaoProfessor.QuestionarioParticipacao != null){
                    query = query.Where(x => x.QuestionarioParticipacao.ID == pRespostaParticipacaoProfessor.QuestionarioParticipacao.ID);
                }
                if (pRespostaParticipacaoProfessor.Professor != null){
                    query = query.Where(x => x.Professor.ID == pRespostaParticipacaoProfessor.Professor.ID);
                }

                if (!string.IsNullOrWhiteSpace(pRespostaParticipacaoProfessor.Resposta))
                    query = query.Where(x => x.Resposta.Contains(pRespostaParticipacaoProfessor.Resposta));
            }

            return query.ToList<RespostaParticipacaoProfessor>();
        }

        #region "Métodos Protected"

        protected override bool ValidarDependencias(object pRespostaParticipacaoProfessor)
        {
            RespostaParticipacaoProfessor respostaParticipacaoProfessor = (RespostaParticipacaoProfessor)pRespostaParticipacaoProfessor;

            return ((respostaParticipacaoProfessor.ListaRespostaParticipacaoOpcoes != null && respostaParticipacaoProfessor.ListaRespostaParticipacaoOpcoes.Count > 0));
        }

        #endregion

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IQueryable<RespostaParticipacaoProfessor> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
