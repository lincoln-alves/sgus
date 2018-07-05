
namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumTipoEtapaPermissao
    {
        Perfil,
        NivelOcupacional,
        Uf,

        // Usuários que recebeão notificações desta etapa.
        NotificarUsuario,
        NotificarSolicitante,
        NotificarChefeImediato,
        NotificarDiretorCorrespondente,
        NotificarGerenteAdjunto,

        // Usuários que irão analisar esta etapa.
        AnalisarUsuario,
        AnalisarSolicitante,
        AnalisarChefeImediato,
        AnalisarDiretorCorrespondente,
        AnalisarGerenteAdjunto
    }
}
