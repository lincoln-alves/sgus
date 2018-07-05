using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Extensions;
using Sebrae.Academico.Dominio.Classes.ConheciGame;

namespace Sebrae.Academico.Dominio.Classes
{

    public class ItemTrilha : EntidadeBasica, ICloneable
    {
        public virtual int QuantidadePontosParticipacao { get; set; }
        //[Obsolete("Essa Solução Educaciona não deve mais ser usava. Para ItemTrilha tipo \"Soluções\", utilize o campo SolucaoEducacionalAtividade")]
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual TrilhaTopicoTematico TrilhaTopicoTematico { get; set; }
        public virtual Missao Missao { get; set; }

        // Usuário que criou a solução Trilheiro
        public virtual Usuario Usuario { get; set; }
        public virtual FormaAquisicao FormaAquisicao { get; set; }
        public virtual IList<ItemTrilhaParticipacao> ListaItemTrilhaParticipacao { get; set; }
        public virtual DateTime? DataCriacao { get; set; }
        public virtual enumStatusSolucaoEducacionalSugerida Aprovado { get; set; }
        public virtual bool? Ativo { get; set; }
        public virtual bool? PermiteReenvioArquivo { get; set; }
        public virtual FileServer FileServer { get; set; }
        public virtual bool? SolucaoObrigatoria { get; set; }
        public virtual Objetivo Objetivo { get; set; }
        public virtual TipoItemTrilha Tipo { get; set; }
        public virtual Questionario Questionario { get; set; }
        public virtual enumFaseJogo FaseJogo { get; set; }
        public virtual SolucaoEducacional SolucaoEducacionalAtividade { get; set; }

        public virtual string ReferenciaBibliografica { get; set; }

        /// <summary>
        /// O campo se chama Local, mas no negócio ele representa as Orientações para participação.
        /// </summary>
        public virtual string Local { get; set; }
        public virtual string Observacao { get; set; }
        public virtual string LinkConteudo { get; set; }
        public virtual int CargaHoraria { get; set; }

        public virtual int? Moedas { get; set; }


        //Não Mapeados
        public virtual bool? UsuarioAssociado { get; set; }
        public virtual string UsuarioAssociadoSimNao
        {
            get
            {
                if (this.Usuario != null && this.Usuario.ID > 0)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }
        public virtual string AprovadoStatus
        {
            get
            {
                switch (Aprovado)
                {
                    case enumStatusSolucaoEducacionalSugerida.NaoAprovado:
                        {
                            return "Não Aprovado";
                        }
                    case enumStatusSolucaoEducacionalSugerida.Aprovado:
                        {
                            return "Aprovado";
                        }
                    default:
                        {
                            return "Aguardando Aprovação";
                        }
                }
            }
        }
        public virtual string AtivoSimNao
        {
            get
            {
                if (this.Ativo.HasValue && this.Ativo.Value)
                {
                    return "Sim";
                }
                else
                {
                    return "Não";
                }
            }
        }

        public virtual string PrazoAvaliacaoTutorFormatado
        {
            get
            {
                if (Missao.PontoSebrae.TrilhaNivel != null)
                {
                    if (Missao.PontoSebrae.TrilhaNivel.PrazoMonitorDiasUteis != null)
                    {
                        var calcularPrazo =
                            DataCriacao.CalcularPrazo((int)Missao.PontoSebrae.TrilhaNivel.PrazoMonitorDiasUteis);
                        if (calcularPrazo != null)
                            return calcularPrazo.Value.ToString("dd/MM/yyyy");
                    }
                }

                return "";
            }
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        public virtual enumStatusParticipacaoItemTrilha? ObterStatusParticipacoesItemTrilha(UsuarioTrilha usuarioTrilha)
        {
            if (Tipo == null)
                return null;

            switch ((enumTipoItemTrilha)Tipo.ID)
            {
                case enumTipoItemTrilha.Solucoes:
                    var participacaoSolucao = ListaItemTrilhaParticipacao
                        .Where(x => x.UsuarioTrilha.ID == usuarioTrilha.ID && x.MatriculaOferta != null)
                        .OrderBy(x => x.MatriculaOferta.ID)
                        .LastOrDefault();

                    if (participacaoSolucao == null || participacaoSolucao.MatriculaOferta == null)
                        return enumStatusParticipacaoItemTrilha.NaoInscrito;

                    //CASO UMA MATRICULA N TIVER APROVADA E A OFERTA N ESTIVER MAIS ABERTA, CONSIDERAR COMO NAO INSCRITO
                    if (!participacaoSolucao.MatriculaOferta.IsAprovado() && !participacaoSolucao.MatriculaOferta.Oferta.IsPrazoConclusao())
                        return enumStatusParticipacaoItemTrilha.NaoInscrito;

                    if (participacaoSolucao.MatriculaOferta.StatusMatricula == enumStatusMatricula.Abandono)
                        return enumStatusParticipacaoItemTrilha.Abandono;

                    if (participacaoSolucao.Autorizado != true && participacaoSolucao.MatriculaOferta.MatriculaTurma.Any(x => x.Turma.DataFinal.HasValue && x.Turma.DataFinal.Value <= DateTime.Now.Date))
                        return enumStatusParticipacaoItemTrilha.Reprovado;

                    if (participacaoSolucao.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito)
                        return enumStatusParticipacaoItemTrilha.EmAndamento;

                    if (participacaoSolucao.Autorizado != true)
                        return enumStatusParticipacaoItemTrilha.EmAndamento;

                    return enumStatusParticipacaoItemTrilha.Aprovado;
                case enumTipoItemTrilha.Discursiva:
                    var participacoes =
                        ListaItemTrilhaParticipacao.Where(itp => itp.UsuarioTrilha.ID == usuarioTrilha.ID)
                            .ToList();

                    if (!participacoes.Any())
                        return enumStatusParticipacaoItemTrilha.NaoInscrito;

                    if (participacoes.Any(x => x.Autorizado == true))
                        return enumStatusParticipacaoItemTrilha.Aprovado;

                    var ultimaParticipacao = participacoes.LastOrDefault();

                    if (ultimaParticipacao != null &&
                        ultimaParticipacao.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro)
                        return enumStatusParticipacaoItemTrilha.Pendente;

                    var ultimaParticipacaoTrilheiro =
                        participacoes.LastOrDefault(
                            x => x.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro);

                    // Se chegou aqui, a última participação pode ser do monitor, então busca a última participação
                    // do monitor e verifica se ela exista e foi autorizada.
                    if (ultimaParticipacao != null && ultimaParticipacaoTrilheiro != null &&
                        ultimaParticipacao.TipoParticipacao == enumTipoParticipacaoTrilha.InteracaoMonitor &&
                        ultimaParticipacaoTrilheiro.Autorizado == false)
                        return enumStatusParticipacaoItemTrilha.Revisar;

                    return enumStatusParticipacaoItemTrilha.NaoInscrito;
                case enumTipoItemTrilha.Jogo:
                case enumTipoItemTrilha.Atividade:
                    // Se for atividade, somente a última participação é que interessa.
                    var participacao =
                        ListaItemTrilhaParticipacao.LastOrDefault(x => x.UsuarioTrilha.ID == usuarioTrilha.ID);

                    if (participacao == null)
                        return enumStatusParticipacaoItemTrilha.NaoInscrito;

                    if (participacao.Autorizado == true)
                        return enumStatusParticipacaoItemTrilha.Aprovado;

                    return enumStatusParticipacaoItemTrilha.EmAndamento;

                case enumTipoItemTrilha.ConheciGame:
                    return enumStatusParticipacaoItemTrilha.EmAndamento;
            }

            return null;
        }

        public virtual bool PodeExibir()
        {
            return Usuario != null ||
                    (
                       Tipo != null &&
                       (Tipo.ID != (int)enumTipoItemTrilha.Atividade ||
                        Questionario != null) &&
                       (Tipo.ID != (int)enumTipoItemTrilha.Solucoes ||
                        SolucaoEducacionalAtividade != null)
                    );
        }

        public virtual string TipoSolucao
        {
            get
            {
                return Usuario == null ? "Sebrae" : "Trilheiro";
            }
        }

        // Tema do conteúdo cadastrado no conhecigame
        public virtual int ID_TemaConheciGame { get; set; }
        public virtual int QuantidadeAcertosTema { get; set; }

        public virtual IList<ItemTrilhaAvaliacao> Avaliacoes { get; set; }

        /// <summary>
        /// Calcula a média de todas as avaliações do item trilha
        /// </summary>
        /// <returns></returns>
        public virtual int ObterMediaAvaliacoes()
        {
            if (Avaliacoes != null)
            {
                int quantidadeAvaliacao = 0;
                int totalAvaliacoes = 0;

                foreach (var avaliacao in Avaliacoes)
                {
                    quantidadeAvaliacao++;
                    totalAvaliacoes += avaliacao.Avaliacao;
                }

                if (quantidadeAvaliacao > 0)
                {
                    return totalAvaliacoes / quantidadeAvaliacao;
                }
            }

            return 0;
        }

        /// <summary>
        /// Retorna true caso o usuário já tenha avaliado o item trilha em questão
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public virtual bool ChecarAvaliacao(UsuarioTrilha usuario)
        {
            if (usuario != null)
            {
                return Avaliacoes.Any(x => x.UsuarioTrilha.ID == usuario.ID);
            }

            return false;
        }
    }
}