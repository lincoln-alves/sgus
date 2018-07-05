
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumStatusParticipacaoItemTrilha
    {
        [Description("Não Inscrito")]
        NaoInscrito = 1,
        [Description("Em Andamento")]
        EmAndamento = 2,
        [Description("Aprovado")]
        Aprovado = 3,
        [Description("Pendente")]
        Pendente = 4,
        [Description("Revisar")]
        Revisar = 5,
        [Description("Abandono")]
        Abandono = 6, //Tipo Solucao Educacional - data final da turma ultrapassa e usuario nao se inscreveu na SE
        [Description("Reprovado")]
        Reprovado = 7 //Tipo Solucao Educacional - data final da turma ultrapassa e usuario inscrito nao conclui a solucao.
    }
}