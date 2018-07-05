using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class NacionalizacaoUf
    {
        public virtual int ID { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual string UsuarioAlteracao { get; set; }
        public virtual DateTime? DataAlteracao { get; set; }
    }
}
