using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class NotificacaoEnvioPermissao : EntidadeBasica
    {
        public virtual NotificacaoEnvio NotificacaoEnvio { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual StatusMatricula Status { get; set; }
    }
}
