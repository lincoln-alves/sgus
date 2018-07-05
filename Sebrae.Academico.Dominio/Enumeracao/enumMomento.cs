
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumMomento
    {
        [Description("Primeiro acesso no mapa da trilha")]
        PrimeiroAcessoMapa = 1,
        [Description("Primeiro acesso na própria mochila")]
        PrimeiroAcessoMochila = 2,
        [Description("Primeira tentativa de câmbio")]
        PrimeiraTentativaCambio = 3,
        [Description("Primeiro acesso à loja no nível")]
        PrimeiroAcessoLoja = 4,
        [Description("Primeira aprovação em uma Solução Sebrae")]
        // Pode ser imediatamente nos casos de soluções que exibem a aprovação na hora, ou posteriormente ao acessar uma loja e haver a nova aprovação
        PrimeiraConclusaoSolucaoSebrae = 5,
        [Description("Primeira conclusão de missão")]
        // Será exibido ao acessar a própria mochila e haver a primeira conclusão da missão.
        PrimeiraConclusaoMissao = 6,
        [Description("Sempre que concluir de todas as Soluções Sebrae de uma loja")]
        ConclusoesTodasSolucoesLoja = 7,
        [Description("Possui a quantidade necessária de moedas para realizar a prova final")]
        PossuirMoedasProvaFinal = 8,
        [Description("Concluir a metade (50%) das Soluções Sebrae de uma loja")]
        ConcluirMetadeSolucoesLoja = 9,
        [Description("Alcançar uma quantidade de troféus suficientes para evoluir a cor do pin")]
        EvoluirPin = 10,
        [Description("Primeiro acesso à criação de Soluções Trilheiro")]
        PrimeiroAcessoCriacaoSolucaoTrilheiro = 11,

        //Daqui para baixo são mensagens alternativas que estão entre as mensagens principais.
        [Description("Primeira atribuição de líder da loja desde o último acesso")]
        PrimeiroLiderLojaUltimoAcesso = 12,

        //Daqui para baixo são mensagens alternativas que estão entre as mensagens principais.
        [Description("Alteração no líder da loja desde o último acesso")]
        AlteracaoLiderLojaUltimoAcesso = 13,

        [Description("Sempre que concluir uma Solução Sebrae após a primeira conclusão")]
        DemaisConclusoesSolucaoSebrae = 14,

        [Description("Sempre que concluir uma missão após a primeira conclusão")]
        DemaisConclusoesMissao = 15,
    }
}
