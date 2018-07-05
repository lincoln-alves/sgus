using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMItemQuestionario : BusinessManagerBase
    {
        #region Atributos

        private RepositorioBase<ItemQuestionario> repositorio = null;

        #endregion

        #region Construtor

        public BMItemQuestionario()
        {
            repositorio = new RepositorioBase<ItemQuestionario>();
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(ItemQuestionario pItemQuestionario)
        {
            ValidarItemQuestionarioInformado(pItemQuestionario);
            repositorio.Salvar(pItemQuestionario);
        }

        public void ValidarItemQuestionarioInformado(ItemQuestionario pItemQuestionario)
        {
            ValidarInstancia(pItemQuestionario);

            //Tipo do Item do Questionário
            if (pItemQuestionario.TipoItemQuestionario == null)
            {
                throw new AcademicoException("Tipo do Item do Questionário. Campo Obrigatório");
            }

            //Texto
            if (string.IsNullOrWhiteSpace(pItemQuestionario.Questao))
            {
                throw new AcademicoException("Texto. Campo Obrigatório");
            }

        }

        public void Excluir(ItemQuestionario pItemQuestionario)
        {
            if (this.ValidarDependencias(pItemQuestionario))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Item de Questionário.");

            repositorio.Excluir(pItemQuestionario);
        }

        public IList<ItemQuestionario> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Questao).ToList<ItemQuestionario>();
        }

        public ItemQuestionario ObterPorID(int pId)
        {
            ItemQuestionario itemQuestionario = null;
            var query = repositorio.session.Query<ItemQuestionario>();
            itemQuestionario = query.FirstOrDefault(x => x.ID == pId);
            return itemQuestionario;
        }

        #endregion

        #region "Métodos Protected"

        protected override bool ValidarDependencias(object pItemQuestionario)
        {
            ItemQuestionario itemQuestionario = (ItemQuestionario)pItemQuestionario;

            return ((itemQuestionario.ListaItemQuestionarioOpcoes != null && itemQuestionario.ListaItemQuestionarioOpcoes.Count > 0));
        }

        #endregion


        public IQueryable<ViewQuestionarioResposta> ObterRespostasPorQuestionario(int idQuestionario, int idTurma = 0)
        {
            var query =
                repositorio.session.Query<ItemQuestionarioParticipacaoOpcoes>()
                    .Where(x =>
                    x.ItemQuestionarioParticipacao.QuestionarioParticipacao.Turma != null &&
                    x.RespostaSelecionada == true &&
                    x.ItemQuestionarioParticipacao.QuestionarioParticipacao.Questionario.ID == idQuestionario);

            if (idTurma > 0)
                query = query.Where(x => x.ItemQuestionarioParticipacao.QuestionarioParticipacao.Turma.ID == idTurma);

            return query.Select(x => new ViewQuestionarioResposta
            {
                ID = x.ID,
                ID_Questionario = x.ItemQuestionarioParticipacao.QuestionarioParticipacao.Questionario.ID,
                ID_Turma = x.ItemQuestionarioParticipacao.QuestionarioParticipacao.Turma.ID,
                ID_ItemQuestionarioParticipacao = x.ItemQuestionarioParticipacao.ID,
                NM_Opcao = x.Nome
            });
        }
    }
}