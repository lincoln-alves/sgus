using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MensageriaParametros : EntidadeBasicaPorId
    {
        public virtual Int16 DiaAviso { get; set; }
        public virtual string NomeArquivoTemplate { get; set; }
        public virtual bool NotificaMatriculaTurma { get; set; }
        public virtual bool NotificaUsuarioTrilha { get; set; }
        public virtual bool Repetir { get; set; }
        public virtual bool EnviarEmail { get; set; }
        public virtual bool EnviarNotificacao { get; set; }

       

    }
}
