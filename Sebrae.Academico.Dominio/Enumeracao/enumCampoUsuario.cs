
using System.ComponentModel;
namespace Sebrae.Academico.Dominio.Enumeracao
{
    /// <summary>
    /// Enumeração referente aos status das respostas de uma etapa
    /// </summary>
    public enum enumCampoUsuario
    {
        [Description("Nome do Usuario")]
        Nome = 0,

        [Description("UF")]
        UF = 1,

        [Description("Nível Ocupacional")]
        NivelOcupacional = 2,

        [Description("Tipo de Documento")]
        TipoDocumento = 3,

        [Description("Mini Currículo")]
        MiniCurriculo = 4,

        [Description("Número de Identidade")]
        NumIdentidade = 5,

        [Description("Orgao Emissor")]
        OrgaoEmissor = 6,

        [Description("Data de Expedição da Identidade")]
        DataExpedicaoIdentidade = 7,

        [Description("Data de Nascimento")]
        DataNascimento = 8,

        [Description("Sexo")]
        Sexo = 9,

        [Description("Nacionalidade")]
        Nacionalidade = 10,

        [Description("Estado Cívil")]
        EstadoCivil = 11,

        [Description("Nome da Mãe")]
        NomeMae = 12,

        [Description("Nome do Pai")]
        NomePai = 13,

        [Description("Número da Matrícula")]
        Matricula = 14,

        [Description("Email")]
        Email = 15,

        [Description("CPF")]
        CPF = 16,

        [Description("Data de Admissão")]
        DataAdmissao = 17,

        [Description("Situação")]
        Situacao = 18,

        [Description("Endereço")]
        Endereco = 19,

        [Description("Bairro")]
        Bairro = 20,

        [Description("CEP")]
        CEP = 21,

        [Description("Estado")]
        Estado = 22,

        [Description("Telefone Residencial")]
        TelResidencial =23,

        [Description("Telefone Celular")]
        TelCelular = 24,

        [Description("Cidade")]
        Cidade = 25,

        [Description("Instituição")]
        Instituicao = 26,

        [Description("País")]
        Pais = 27,

        [Description("Complemento")]
        Complemento = 28,

        [Description("Escolaridade")]
        Escolaridade = 29,

        [Description("Campo de Conhecimento")]
        CampoConhecimento = 30,

        [Description("Tipo de Instituição")]
        TipoInstituicao = 31,

        [Description("Ano de Conclusão")]
        AnoConclusao = 32,

        [Description("Naturalidade")]
        Naturalidade = 33,

        [Description("Ramal")]
        Ramal = 34,

        [Description("Unidade")]
        Unidade = 35,

        [Description("Histórico Acadêmico")]
        HistoricoAcademico = 36
        
    }
}
