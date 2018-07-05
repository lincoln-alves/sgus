using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMRespostaParticipacaoProfessorOpcoes : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<RespostaParticipacaoProfessorOpcoes> repositorio;

        public BMRespostaParticipacaoProfessorOpcoes()
        {
            repositorio = new RepositorioBase<RespostaParticipacaoProfessorOpcoes>();
        }

        public void ValidarItemQuestionarioInformado(RespostaParticipacaoProfessorOpcoes pRespostaParticipacaoProfessorOpcoes)
        {
            ValidarInstancia(pRespostaParticipacaoProfessorOpcoes);
        }

        public void Salvar(RespostaParticipacaoProfessorOpcoes pRespostaParticipacaoProfessorOpcoes)
        {
            ValidarItemQuestionarioInformado(pRespostaParticipacaoProfessorOpcoes);
            repositorio.Salvar(pRespostaParticipacaoProfessorOpcoes);
        }
        public void Excluir(RespostaParticipacaoProfessorOpcoes pRespostaParticipacaoProfessorOpcoes)
        {
            //if (this.ValidarDependencias(pRespostaParticipacaoProfessorOpcoes))
            //    throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta informação.");

            repositorio.Excluir(pRespostaParticipacaoProfessorOpcoes);
        }

        public IList<RespostaParticipacaoProfessorOpcoes> ObterTodos()
        {
            return repositorio.ObterTodos().ToList<RespostaParticipacaoProfessorOpcoes>();
        }

        public IList<RespostaParticipacaoProfessorOpcoes> ObterPorFiltro(RespostaParticipacaoProfessorOpcoes pRespostaParticipacaoProfessorOpcoes)
        {
            var query = repositorio.session.Query<RespostaParticipacaoProfessorOpcoes>();

            if (pRespostaParticipacaoProfessorOpcoes != null)
            {
                if (pRespostaParticipacaoProfessorOpcoes.ItemQuestionarioParticipacao != null)
                    query = query.Where(x => x.ItemQuestionarioParticipacao.ID == pRespostaParticipacaoProfessorOpcoes.ItemQuestionarioParticipacao.ID);
                if (pRespostaParticipacaoProfessorOpcoes.ItemQuestionarioParticipacaoOpcoes != null)
                    query = query.Where(x => x.ItemQuestionarioParticipacaoOpcoes.ID == pRespostaParticipacaoProfessorOpcoes.ItemQuestionarioParticipacaoOpcoes.ID);
                if (pRespostaParticipacaoProfessorOpcoes.RespostaParticipacaoProfessor != null)
                    query = query.Where(x => x.RespostaParticipacaoProfessor.ID == pRespostaParticipacaoProfessorOpcoes.RespostaParticipacaoProfessor.ID);
            }

            return query.ToList<RespostaParticipacaoProfessorOpcoes>();
        }

        public RespostaParticipacaoProfessorOpcoes ObterPorID(int pId)
        {
            RespostaParticipacaoProfessorOpcoes respostaParticipacaoProfessorOpcoes = null;
            var query = repositorio.session.Query<RespostaParticipacaoProfessorOpcoes>();
            respostaParticipacaoProfessorOpcoes = query.FirstOrDefault(x => x.ID == pId);
            return respostaParticipacaoProfessorOpcoes;
        }

        #region "Métodos Protected"

        /*protected override bool ValidarDependencias(object pRespostaParticipacaoProfessor)
        {
            RespostaParticipacaoProfessorOpcoes respostaParticipacaoProfessor = (RespostaParticipacaoProfessorOpcoes)pRespostaParticipacaoProfessor;

            return ((respostaParticipacaoProfessor.ListaRespostaParticipacaoOpcoes != null && respostaParticipacaoProfessor.ListaRespostaParticipacaoOpcoes.Count > 0));
        }*/

        #endregion

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IQueryable<RespostaParticipacaoProfessorOpcoes> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
