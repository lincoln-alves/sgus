using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MensageriaRegistro : EntidadeBasicaPorId
    {
        public virtual MensageriaParametros MensageriaParametro { get; set; }
        public virtual MatriculaTurma MatriculaTurma { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual DateTime DataEnvio { get; set; }
        public virtual string TextoEnviado { get; set; }
        public virtual Usuario Usuario { get; set; }
        
    }
}
