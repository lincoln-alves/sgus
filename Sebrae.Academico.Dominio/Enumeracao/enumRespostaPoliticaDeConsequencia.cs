namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumRespostaPoliticaDeConsequencia
    {
        // Apto a se inscrever pelas políticas de consequência.
        AptoInscricao,
        // Usuário foi aprovado na SE no período de 1 ano.
        Aprovado,
        // Cancelou antes de 1/3 da duração do curso.
        Cancelamento,
        // Abandonou o curso (não cancelou e nem participou até a conclusão).
        Abandono,
        // Foi reprovado.
        Reprovado
    }
}
