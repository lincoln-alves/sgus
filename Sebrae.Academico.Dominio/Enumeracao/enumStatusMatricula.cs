
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumStatusMatricula
    {
        [Description("Inscrito")]
        Inscrito = 2,

        [Description("Cancelao/Aluno")]
        CanceladoAluno = 3,

        [Description("Cancelado/Adm")]
        CanceladoAdm = 4,

        [Description("Abandono/Desistente")]
        Abandono = 5,

        [Description("Pendente confirmação aluno")]
        PendenteConfirmacaoAluno = 6,

        [Description("Concluído")]
        Concluido = 7,

        [Description("Fila de espera")]
        FilaEspera = 8,

        [Description("Aprovado")]
        Aprovado = 9,

        [Description("Reprovado")]
        Reprovado = 10,

        [Description("Aprovado como Multiplicador")]
        AprovadoComoMultiplicador = 13,

        [Description("Aprovado como Multiplicador com Acompanhamento")]
        AprovadoComoMultiplicadorComAcompanhamento = 14,

        [Description("Aprovado como Facilitador")]
        AprovadoComoFacilitador = 15,

        [Description("Aprovado como Facilitador o Acompanhamento")]
        AprovadoComoFacilitadorComAcompanhamento = 16,

        [Description("Aprovado como Consultor")]
        AprovadoComoConsultor = 17,

        [Description("Aprovado como Consultor com Acompanhamento")]
        AprovadoComoConsultorComAcompanhamento = 18,

        [Description("Aprovado como Moderador")]
        AprovadoComoModerador = 19,

        [Description("Aprovado como Moderador com Acompanhamento")]
        AprovadoComoModeradorComAcompanhamento = 20,

        [Description("Aprovado como Facilitador Consultor")]
        AprovadoComoFacilitadorConsultor = 22,

        [Description("Cancelado/Gestor")]
        CanceladoGestor = 23,

        [Description("Aprovado como Gestor")]
        AprovadoComoGestor = 24,

        [Description("Aprovado como Facilitador Consultor com Acompanhamento")]
        AprovadoComoFacilitadorConsultorComAcompanhamento = 25,

        [Description("Cancelado pela turma")]
        CanceladoTurma = 26,

        [Description("Ouvinte")]
        Ouvinte = 27,

        [Description("ReprovadoTempo")]
        ReprovadoTempo = 28,

        [Description("AbandonoTempo")]
        AbandonoTempo = 29
    }
}
