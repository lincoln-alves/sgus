using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumAcaoNaPagina
    {
        [Description("Visualização")]
        Visualizacao = 1,
        [Description("Criação")]
        Criacao = 2,
        [Description("Edição")]
        Edicao = 3,
        [Description("Exclusão")]
        Exclusao = 4,
        [Description("Edição de Matrícula")]
        EdicaoMatricula = 5,
        [Description("Inscrição de aluno")]
        IncricaoAluno = 6
    }
}
