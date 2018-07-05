using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Linq;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Extensions;

namespace Sebrae.Academico.BP
{
    public class ManterQuestionarioAssociacaoEnvio : BusinessProcessBase
    {
        private BMQuestionarioAssociacaoEnvio bmQuestionarioAssociacaoEnvio = null;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterQuestionarioAssociacaoEnvio()
            : base()
        {
            bmQuestionarioAssociacaoEnvio = new BMQuestionarioAssociacaoEnvio();
        }


        // Obter todos no intervalo determinado, ativos e que ainda ao responderam o questionario.
        public IQueryable<QuestionarioAssociacaoEnvio> ObterPorIntervalo(int intervalo)
        {
            DateTime inicio = DateTime.Now.AddMinutes(-intervalo);
            DateTime fim = DateTime.Now;

            IQueryable<QuestionarioAssociacaoEnvio> query = bmQuestionarioAssociacaoEnvio.ObterTodosIQueryable()
                .Where(
                    x => (x.DataEnvio >= inicio && x.DataEnvio <= fim) && x.Ativo == true
                    && !x.QuestionarioAssociacao.Questionario.ListaQuestionarioParticipacao
                        .Any(
                            qp => qp.Usuario.ID == x.Usuario.ID
                            && !qp.DataParticipacao.HasValue
                            && qp.Questionario.ID == x.QuestionarioAssociacao.Questionario.ID
                        )
                );
            
            return query;
        }

        //Inserir registro de acordo com a Quantidade maxima de envio calculando o intervalo de envio  de 7 em 7 dias
        public void Inserir(MatriculaTurma matriculaTurma)
        {
            int qtdMaxEnvio = 3;
            int qtdIntervaloEnvio = 7;
            DateTime agora = DateTime.Now;

            for (int i = 0; i < qtdMaxEnvio; i++)
            {

                double qtdDia = (i == 0) ? i : (qtdIntervaloEnvio * i);

                var questionarioAssociacaoEnvio = new QuestionarioAssociacaoEnvio();
                questionarioAssociacaoEnvio.Ativo = true;
                questionarioAssociacaoEnvio.DataEnvio = agora.AddDays(qtdDia);
                questionarioAssociacaoEnvio.Usuario = matriculaTurma.MatriculaOferta.Usuario;
                questionarioAssociacaoEnvio.QuestionarioAssociacao = matriculaTurma.Turma.ListaQuestionarioAssociacao.FirstOrDefault(
                        y => y.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos);

                bmQuestionarioAssociacaoEnvio.Salvar(questionarioAssociacaoEnvio);
            }
        }

        //
        public void Atualizar(QuestionarioAssociacaoEnvio questionarioAssociacaoEnvio)
        {
            //nao quero executar todo o comando no lote
            questionarioAssociacaoEnvio.Ativo = false;
            bmQuestionarioAssociacaoEnvio.Salvar(questionarioAssociacaoEnvio);
        }

        //Inativar
        public void Inativar(int intervalo)
        {
            DateTime inicio = DateTime.Now.AddMinutes(-intervalo);
            DateTime fim = DateTime.Now;

            //Inativar Log quando o questionario já tiver sido respondido
            var questionariosAssociacaoEnvio = bmQuestionarioAssociacaoEnvio.ObterTodosIQueryable()
                .Where(
                    x => x.Ativo == true
                    && (x.DataEnvio >= inicio && x.DataEnvio <= fim)
                    && !x.QuestionarioAssociacao.Questionario.ListaQuestionarioParticipacao
                        .Any(
                            qp => qp.Usuario.ID == x.Usuario.ID
                            && qp.DataParticipacao.HasValue
                            && qp.Questionario.ID == x.QuestionarioAssociacao.Questionario.ID
                        )
                );

            foreach (var questionarioAssociacaoEnvio in questionariosAssociacaoEnvio)
            {
                Atualizar(questionarioAssociacaoEnvio);
            }
        }
    }
}
