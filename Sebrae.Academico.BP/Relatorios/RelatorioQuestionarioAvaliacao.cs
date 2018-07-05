using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioQuestionarioAvaliacao : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.QuestionarioAvaliacao; }
        }

        public IList<DTOQuestionarioAvaliacao> ConsultarQuestionarioAvaliacao(ItemTrilha pTrilha)
        {
            IList<DTOQuestionarioAvaliacao> retorno = null;

            
            //TB_ITEM_QUESTIONARIO_PARTICIPACAO
            //TB_ItemQuestionarioParticipacaoOpcoes

            var questionarioComParticipacao = new BMQuestionarioParticipacao().ObterTodosQuestionariosComParticipacao();
            List<int> cpfsParticipantes = new BMQuestionarioParticipacao().ObterTodosUsuarios().ToList();

            Usuario filtro = new Usuario();
            var usuarioQueResponderam = new BMUsuario().ObterUsuariosQueResponderamItensTrilha(filtro, cpfsParticipantes);

            foreach (var item in questionarioComParticipacao)
            {
                
            }

            foreach (var usuario in usuarioQueResponderam)
            {
                foreach (var itemTrilha in usuario.ListaMatriculasNaTrilha.ToList())
                {
                    foreach (var itemTrilhaParticipacao in itemTrilha.ListaItemTrilhaParticipacao.ToList())
                    {
                        DTOQuestionarioAvaliacao m = new DTOQuestionarioAvaliacao();

                        m.Questionario.Add(new DTOQuestionarioAvaliacaoQuestionario { Questionario = itemTrilhaParticipacao.ItemTrilha.Descricao, Nota = (itemTrilhaParticipacao.UsuarioTrilha.QTEstrelas.HasValue ? int.Parse(itemTrilhaParticipacao.UsuarioTrilha.QTEstrelas.Value.ToString()) : 0) });
                        retorno.Add(m);
                    }                    
                }
            }
           
            return retorno;
        }


        public IList<Questionario> ObterQuestionario()
        {
            BMQuestionario questionarioBM = new BMQuestionario();
            return questionarioBM.ObterTodos();
        }

        public IList<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (BMFormaAquisicao formaBM = new BMFormaAquisicao())
            {
                return formaBM.ObterTodos();
            }
        }

        public IList<SolucaoEducacional> ObterSolucaoEducacional()
        {
            using (var solucaoEducacionalBm = new BMSolucaoEducacional())
            {
                return solucaoEducacionalBm.ObterTodos().ToList();
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
