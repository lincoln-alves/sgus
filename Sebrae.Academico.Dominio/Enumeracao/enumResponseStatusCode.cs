
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    /// <summary>
    /// 0: Sucesso
    /// 1: Usuário não autenticado.
    /// 2: Mapa não encontrado
    /// 3: Usuário não matriculado
    /// 4 a 9: Reservado para futuros erros genéricos.
    /// 10 a 99: Erro contextual, ou seja, que varia de acordo com a requisição.
    /// 100: Erro de negócio, ou seja, que foi gerados a partir de regras específicas e pré-existentes do negócio.
    /// </summary>
    public enum enumResponseStatusCode
    {
        [Description("Sucesso")]
        Sucesso = 0,

        [Description("Usuário não autenticado")]
        UsuarioNaoAutenticado = 1,

        [Description("Mapa não encontrado")]
        MapaNaoEncontrado = 2,

        [Description("Usuário não matriculado")]
        UsuarioNaoMatriculado = 3,

        [Description("Registro não encontrado")]
        RegistroNaoEncontrado = 4,

        [Description("Regra de negócio, mensagem de erro deve ser customizada")]
        ErroRegraNegocioTrilhas = 5,

        [Description("Erro de preenchimento de formulário: Campos obrigatórios não preenchidos")]
        ErroCampoObrigatorio = 6,

        [Description("Questionário não existente no nível da trilha.")]
        QuestionarioNaoEncontrado = 11,

        [Description("ID da Solução Sebrae não informado.")]
        SolucaoSebraeNaoInformada = 12,

        [Description("Tipo de questionário inválido.")]
        TipoQuestionarioInvalido = 13,

        [Description("Solução Sebrae não vinculada ao questionário respondido.")]
        SolucaoSebraeNaoVinculada = 21,

        [Description("Questionário inválido")]
        QuestionarioInvalido = 22,

        [Description("Turma não encontrada.")]
        TurmaNaoEncontrada = 23,

        [Description("Solução Sebrae não encontrada.")]
        SolucaoSebraeNaoEncontrada = 24,

        [Description("Solução Educacional não encontrada.")]
        SolucaoEducacionalNaoEncontrada = 25,

        [Description("Turma não informada.")]
        TurmaNaoInforma = 26,

        [Description("Oferta não encontrada.")]
        OfertaNaoEncontrada = 27,

        [Description("Usuário não pode curtir a própria solução.")]
        CurtirSolucaoPropria = 28,

        [Description("Entrevista respondida.")]
        EntrevistaRespondida = 29,

        [Description("Você ainda não cruzou a linha de chegada. Continue jogando.")]
        EntrevistaBloqueada = 30,

        [Description("Parabéns {0} você concluiu com êxito sua Prova Final.")]
        UsuarioAprovadoProvaFinal = 31,

        [Description("Certificado inexistente")]
        CertificadoInexistente = 32,

        [Description("Usuário não aprovado")]
        UsuarioNaoAprovado = 33,

        [Description("Permissão negada para Super Acesso")]
        PermissaoNegadaSuperAcesso = 34,

        [Description("Posição no Ranking não encontrada")]
        PosicaoRankingNaoEntontrada = 35,

        [Description("Ponto Sebrae não encontrado")]
        PontoSebraeNaoEncontrado = 36,

        [Description("Erro de negócio: gerado a partir de regras específicas e pré-existentes do negócio")]
        ErroRegraNegocioSgus = 100,
    }
}
