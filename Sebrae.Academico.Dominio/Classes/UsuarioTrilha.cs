using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioTrilha : EntidadeBasicaComStatus
    {
        public virtual TrilhaNivel TrilhaNivel { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime DataLimite { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual decimal? NotaProva { get; set; }
        //public virtual DateTime? DataUltimoAcesso { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual IList<ItemTrilhaParticipacao> ListaItemTrilhaParticipacao { get; set; }

        public virtual IList<TrilhaAtividadeFormativaParticipacao> ListaTrilhaAtividadeFormativaParticipacao { get; set; }

        public virtual IList<UsuarioTrilhaMoedas> ListaUsuarioTrilhaMoedas { get; set; }
        public virtual IList<Notificacao> ListaNotificacoes { get; set; }
        public virtual IList<UsuarioTrilhaMensagemGuia> ListaVisualizacoesMensagemGuia { get; set; }
        public virtual IList<LogLider> ListaLogLider { get; set; }

        public virtual string CDCertificado { get; set; }
        public virtual DateTime? DataGeracaoCertificado { get; set; }
        public virtual Boolean NovaProvaLiberada { get; set; }
        public virtual DateTime? DataLiberacaoNovaProva { get; set; }
        public virtual DateTime? DataAlteracaoStatus { get; set; }

        public virtual Boolean AcessoBloqueado { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual byte? QTEstrelas { get; set; }
        public virtual byte? QTEstrelasPossiveis { get; set; }

        public virtual bool? NovasTrilhas { get; set; }

        public virtual enumTipoTrofeu TipoTrofeu { get; set; }

        public virtual string Token { get; set; }

        public virtual bool AprovadoProvaFinal
        {
            get
            {
                //verifica se o usuario possui uma prova final concluida.
                return
                    this.TrilhaNivel.ListaQuestionarioParticipacao.Any(
                        x =>
                            x.Usuario.ID == this.Usuario.ID && x.DataParticipacao != null &&
                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova &&
                            x.IsAprovado());
            }
        }

        public virtual bool TeveParticipacao
        {
            get
            {
                //verifica se o usuario possui uma participação em alguma solução sebrae
                return this.ListaItemTrilhaParticipacao.Any(x => x.UsuarioTrilha.ID == this.ID);
            }
        }

        /// <summary>
        /// Variável para manter o nível do usuário no objeto para não ter que calcular tudo novamente.
        /// </summary>
        private int? Nivel { get; set; }

        /// <summary>
        /// Obtém o nível do usuário baseado nas moedas obtidas.
        /// </summary>
        /// <param name="moedasOuro">Quantidade de moedas de ouro do usuário. Precisa ser calculado fora pra evitar lazy e lentidão</param>
        /// <param name="totalMoedasNivel">Total de moedas de ouro disponíveis em todas as SS do nível.</param>
        /// <returns></returns>
        public virtual int ObterNivel(int moedasOuro, int totalMoedasNivel)
        {
            if (Nivel.HasValue)
            {
                return Nivel.Value;
            }

            var nivelAtual = 1;
            var moedasPorNivel = 0;
            var minMoedasNivel = 0;
            var calculos = ObterCalculosTrofeus(totalMoedasNivel);

            switch (ObterTipoTrofeu(moedasOuro, totalMoedasNivel))
            {
                case enumTipoTrofeu.Bronze:
                    moedasPorNivel = calcularPorcentoDe(20, calculos[enumTipoTrofeu.Bronze]);
                    break;
                case enumTipoTrofeu.Prata:
                    nivelAtual += 5;

                    minMoedasNivel = calculos[enumTipoTrofeu.Bronze];

                    moedasPorNivel = calcularPorcentoDe(20, calculos[enumTipoTrofeu.Prata]);
                    break;
                case enumTipoTrofeu.Ouro:
                    nivelAtual += 10;

                    minMoedasNivel = calculos[enumTipoTrofeu.Bronze] + calculos[enumTipoTrofeu.Prata];

                    moedasPorNivel = calcularPorcentoDe(20, calculos[enumTipoTrofeu.Ouro]);
                    break;
            }

            if (moedasOuro <= 0 || moedasPorNivel <= 0 || minMoedasNivel <= 0)
                return nivelAtual;

            while ((moedasOuro - moedasPorNivel) >= minMoedasNivel)
            {
                moedasOuro = (moedasOuro - moedasPorNivel);
                nivelAtual++;

                if (nivelAtual == 15)
                    break;
            }

            Nivel = nivelAtual;

            return nivelAtual;
        }

        public virtual Dictionary<enumTipoTrofeu, int> ObterCalculosTrofeus(int totalMoedasNivel)
        {
            var porcentagensTrofeus =
                Array.ConvertAll((TrilhaNivel.PorcentagensTrofeus ?? "33,66").Split(','),
                    int.Parse);

            return new Dictionary<enumTipoTrofeu, int>()
            {
                {
                    enumTipoTrofeu.Bronze, calcularPorcentoDe(porcentagensTrofeus[0], totalMoedasNivel)
                },
                {
                    enumTipoTrofeu.Prata,
                    calcularPorcentoDe(porcentagensTrofeus[1] - porcentagensTrofeus[0], totalMoedasNivel)
                },
                {
                    enumTipoTrofeu.Ouro, calcularPorcentoDe(100 - porcentagensTrofeus[1], totalMoedasNivel)
                }
            };
        }

        /// <summary>
        /// Obtém o tipo de troféu do usuário.
        /// </summary>
        /// <param name="moedasOuro">Quantidade de moedas de ouro do usuário. Precisa ser calculado fora pra evitar lazy e lentidão</param>
        /// <param name="totalMoedasNivel">Total de moedas de ouro disponíveis em todas as SS do nível.</param>
        /// <returns></returns>
        public virtual enumTipoTrofeu ObterTipoTrofeu(int moedasOuro, int totalMoedasNivel)
        {
            var porcentagensTrofeus = Array.ConvertAll((TrilhaNivel.PorcentagensTrofeus ?? "33,66").Split(','),
                int.Parse);

            var porcentagemAtual = calcularPorcentagemDe(moedasOuro, totalMoedasNivel);

            if (porcentagemAtual < porcentagensTrofeus[0])
                return enumTipoTrofeu.Bronze;
            if (porcentagemAtual >= porcentagensTrofeus[0] && porcentagemAtual <= porcentagensTrofeus[1])
                return enumTipoTrofeu.Prata;

            return enumTipoTrofeu.Ouro;
        }

        private int calcularPorcentagemDe(int valor, int total)
        {
            return (valor > 0 && total > 0)
                ? Convert.ToInt32(Math.Round((100 * Convert.ToDouble(valor)) / Convert.ToDouble(total)))
                : 0;
        }

        private int calcularPorcentoDe(int porcentagem, int total)
        {
            return (porcentagem > 0 && total > 0)
                ? Convert.ToInt32(Math.Round((Convert.ToDouble(porcentagem) / 100) * Convert.ToDouble(total)))
                : 0;
        }

        #region "Atributos que não serão mapeados"

        public virtual string DataLimiteFormatada
        {
            get { return this.DataLimite.ToShortDateString(); }
        }

        public virtual string DataInicioFormatada
        {
            get { return this.DataInicio.ToShortDateString(); }
        }

        public virtual string DataFimFormatada
        {
            get
            {
                string dataFimFormatada = string.Empty;

                if (DataFim.HasValue)
                {
                    dataFimFormatada = this.DataFim.Value.ToShortDateString();
                }

                return dataFimFormatada;
            }
        }

        #endregion

        public virtual TimeSpan Cronometro
        {
            get { return DataLimite.Subtract(DateTime.Now); }
        }

        public virtual int ObterCurtidasPorTipo(ItemTrilha itemTrilha, enumTipoCurtida tipoCurtida)
        {
            return
                ListaUsuarioTrilhaMoedas.Count(
                    m => m.Curtida != null && m.Curtida.ItemTrilha.ID == itemTrilha.ID &&
                        m.TipoCurtida == tipoCurtida);
        }

        public virtual int ObterSomaMoedasPrata(ItemTrilha solucaoTrilheiro)
        {
            return
                ListaUsuarioTrilhaMoedas.Where(m => m.Curtida != null && m.Curtida.ItemTrilha.ID == solucaoTrilheiro.ID)
                    .Sum(x => x.MoedasDePrata);
        }

        public virtual int ObterSomaMoedas(enumTipoMoeda tipoMoeda)
        {
            switch (tipoMoeda)
            {
                case enumTipoMoeda.Prata:
                    return ListaUsuarioTrilhaMoedas.Sum(x => x.MoedasDePrata);
                case enumTipoMoeda.Ouro:
                    return ListaUsuarioTrilhaMoedas.Sum(x => x.MoedasDeOuro);
                default:
                    throw new ArgumentOutOfRangeException("tipoMoeda");
            }
        }        
        public virtual int ObterSomaMoedasNivel(enumTipoMoeda tipoMoeda, TrilhaNivel nivel)
        {

            return ListaUsuarioTrilhaMoedas.Where(
                x =>
                    x.UsuarioTrilha.TrilhaNivel.ID == nivel.ID && x.ItemTrilha != null)
                .Sum(x => tipoMoeda == enumTipoMoeda.Ouro ? x.MoedasDeOuro : x.MoedasDePrata);
        }

        public virtual bool PossuiSaldoProvaFinal()
        {
            return ObterSomaMoedas(enumTipoMoeda.Ouro) >= (TrilhaNivel.QuantidadeMoedasProvaFinal ?? 0);
        }

        /// <summary>
        /// Formatar o tempo de conclusão do líder no formato 5d 24h 60m 60s
        /// </summary>
        /// <param name="tempo">TimeSpan da participação mais antiga até a última participação.</param>
        /// <returns></returns>
        public virtual string ObterTempoConclusaoFormatado(TimeSpan? tempo)
        {
            if (tempo.HasValue)
            {
                var retorno = "";

                if (tempo.Value.Days > 0)
                    retorno += tempo.Value.Days + "d ";

                if (tempo.Value.Hours > 0)
                    retorno += tempo.Value.Hours + "h ";

                if (tempo.Value.Minutes > 0)
                    retorno += tempo.Value.Minutes + "m ";

                if (tempo.Value.Seconds > 0)
                    retorno += tempo.Value.Seconds + "s";

                return retorno;
            }

            return "0s";
        }
    }
}