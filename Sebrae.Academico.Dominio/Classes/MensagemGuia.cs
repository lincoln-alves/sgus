using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MensagemGuia
    {
        public virtual enumMomento ID { get; set; }

        public virtual int ID_INT
        {
            get { return (int) ID; }
        }

        public virtual enumTipoMensagemGuia Tipo { get; set; }
        public virtual IEnumerable<TrilhaTutorial> Tutoriais { get; set; }
        public virtual string Texto { get; set; }
        public virtual IList<UsuarioTrilhaMensagemGuia> ListaUsuarioTrilhaMensagemGuia { get; set; }
        public virtual string HashTags { get; set; }

        public virtual List<string> ObterHashTags()
        {
            if (HashTags == null)
                return null;

            return HashTags.Split(',')
                .Select(x =>
                    x.StartsWith("#")
                        ? x.Substring(1, x.Length - 1)
                        : x)
                .ToList();
        }

        public virtual bool DeveExibirMensagem(UsuarioTrilha matricula)
        {
            return ListaUsuarioTrilhaMensagemGuia.All(x => x.UsuarioTrilha.ID != matricula.ID);
        }

        public virtual string ObterTexto(Trilha trilha, UsuarioTrilha matricula, LogLider logLider = null,
            ItemTrilha solucaoSebrae = null, Missao missao = null, PontoSebrae pontoSebrae = null, string corPin = null,
            TrilhaTutorial trilhaTutorial = null)
        {
            try
            {
                // Caso tenha um tutorial vinculado
                if (Tipo == enumTipoMensagemGuia.Tutorial && trilhaTutorial != null )
                {
                    var tutorial = Tutoriais.FirstOrDefault(x => x.ID == trilhaTutorial.ID);

                    if(tutorial != null && !string.IsNullOrEmpty(tutorial.Conteudo))
                        return tutorial.Conteudo;
                }

                // Para cada momento, precisa obter os dados personalizados de acordo com a necessidade.
                string texto;

                switch (ID)
                {
                    // Esses cases aqui são os que não precisam concatenar nenhum dado. Se isso mudar um dia,
                    // tem que mudar aqui.
                    case enumMomento.PrimeiroAcessoMapa:
                    case enumMomento.PrimeiroAcessoMochila:
                    case enumMomento.PrimeiraTentativaCambio:
                    case enumMomento.PossuirMoedasProvaFinal:
                    case enumMomento.PrimeiroAcessoCriacaoSolucaoTrilheiro:
                        texto = Texto;
                        break;

                    case enumMomento.PrimeiroAcessoLoja:
                        if (pontoSebrae == null)
                            throw new Exception("Ponto Sebrae é obrigatório.");

                        texto = Texto.Replace("#NOME_PONTO_SEBRAE", pontoSebrae.NomeExibicao);

                        break;

                    case enumMomento.PrimeiroLiderLojaUltimoAcesso:
                    case enumMomento.AlteracaoLiderLojaUltimoAcesso:
                        if (logLider == null || logLider.Lider == null || logLider.Tempo == null)
                        {
                            texto = Texto;
                            break;
                        }

                        texto = string.Format(Texto, logLider.Lider.Usuario.Nome,
                            logLider.Tempo != null ? logLider.Lider.ObterTempoConclusaoFormatado(logLider.Tempo) : "0s");

                        if (pontoSebrae == null)
                            throw new Exception("Ponto Sebrae é obrigatório.");

                        texto = texto.Replace("#NOME_PONTO_SEBRAE", pontoSebrae.NomeExibicao);
                        break;

                    case enumMomento.PrimeiraConclusaoSolucaoSebrae:
                        if (solucaoSebrae == null)
                            throw new Exception("Solução Sebrae é obrigatória.");

                        texto = string.Format(Texto, solucaoSebrae.Tipo.Nome, solucaoSebrae.Moedas ?? 0,
                            solucaoSebrae.Missao.PontoSebrae.TrilhaNivel.QuantidadeMoedasProvaFinal ?? 0);

                        texto = texto.Replace("#NOME_SOLUCAO_SEBRAE", solucaoSebrae.Nome);
                        break;

                    // A mensagem é a mesma para a primeira e as demais missões concluídas. Se isso mudar um dia,
                    // tem que mudar aqui.
                    case enumMomento.PrimeiraConclusaoMissao:
                    case enumMomento.DemaisConclusoesMissao:
                        if (missao == null)
                            throw new Exception("Missão é obrigatória.");

                        texto = Texto.Replace("#NOME_PONTO_SEBRAE", missao.Nome);
                        break;

                    // Os cases abaixo estão juntos porque o dado concatenado é idêntico nos dois casos. Se isso
                    // mudar um dia, tem que mudar aqui.
                    case enumMomento.ConclusoesTodasSolucoesLoja:
                    case enumMomento.ConcluirMetadeSolucoesLoja:
                        if (solucaoSebrae == null)
                            throw new Exception("Solução Sebrae é obrigatória.");

                        if (pontoSebrae == null)
                            throw new Exception("Ponto Sebrae é obrigatório.");

                        texto = Texto.Replace("#NOME_PONTO_SEBRAE", pontoSebrae.NomeExibicao);
                        break;

                    case enumMomento.EvoluirPin:
                        texto = string.Format(Texto, corPin);
                        break;

                    case enumMomento.DemaisConclusoesSolucaoSebrae:
                        if (solucaoSebrae == null)
                            throw new Exception("Solução Sebrae é obrigatória.");

                        if (pontoSebrae == null)
                            throw new Exception("Ponto Sebrae é obrigatório.");

                        texto = string.Format(Texto, solucaoSebrae.Tipo.Nome, solucaoSebrae.Moedas ?? 0);

                        texto = texto.Replace("#NOME_PONTO_SEBRAE", pontoSebrae.NomeExibicao);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (trilha == null)
                    throw new Exception("A trilha é obrigatória para gerar a mensagem da guia");

                if (matricula == null)
                    throw new Exception("A matrícula é obrigatória para gerar a mensagem da guia");

                texto = texto
                    .Replace("#NOME_ALUNO", matricula.Usuario.Nome)
                    .Replace("#NOME_TRILHA", trilha.Nome)
                    .Replace("#NOME_NIVEL", matricula.TrilhaNivel.Nome)
                    .Replace("#DATA_MATRICULA", matricula.DataInicio.ToShortDateString())
                    .Replace("#DATA_LIMITE", matricula.DataLimite.ToShortDateString());

                return texto;
            }
            catch (Exception ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }
    }
}